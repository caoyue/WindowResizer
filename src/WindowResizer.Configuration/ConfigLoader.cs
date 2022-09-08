using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Configuration;

public static class ConfigLoader
{
    private const string ConfigFile = $"{nameof(WindowResizer)}.config.json";

    private static readonly string RoamingPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer));

    private static string _roamingConfigPath = Path.Combine(RoamingPath, ConfigFile);

    private static string _portableConfigPath = string.Empty;

    private const string DefaultProfileName = "default";

    public static bool PortableMode => File.Exists(RoamingPath);

    public static void SetPath(string roamingPath, string portablePath)
    {
        _roamingConfigPath = roamingPath;
        _portableConfigPath = portablePath;
    }

    public static string ConfigPath => PortableMode ? _portableConfigPath : _roamingConfigPath;

    public static readonly Config Config = new();

    public static readonly Profiles Profiles = new();

    public static void Load()
    {
        if (!File.Exists(ConfigPath))
        {
            UseDefault();

            Save();
            return;
        }

        Load(ConfigPath);
    }

    public static void UseDefault()
    {
        Profiles.Configs.Clear();

        Config.Keys.Clear();
        foreach (var key in Config.DefaultKeys)
        {
            Config.Keys.Add(key.Key, key.Value);
        }


        Config.DisableInFullScreen = true;
        Config.WindowSizes.Clear();
        Config.CheckUpdate = true;

        Config.ProfileName = DefaultProfileName;
        Config.IsCurrent = true;
        Profiles.Configs.Add(Config);
    }

    public static void Load(string path)
    {
        Profiles.Configs.Clear();

        var text = File.ReadAllText(path);
        var p = JsonConvert.DeserializeObject<Profiles>(text);
        if (p?.Configs is null || !p.Configs.Any())
        {
            MigrateToProfile(path);
            return;
        }

        foreach (var config in p.Configs)
        {
            Profiles.Configs.Add(config);
        }

        UseConfig(Profiles.Current);
    }

    public static void Save()
    {
        var json = JsonConvert.SerializeObject(Profiles);
        new FileInfo(ConfigPath).Directory?.Create();
        File.WriteAllText(ConfigPath, json);
    }

    public static void Move(bool portable)
    {
        if (portable && !PortableMode)
        {
            File.Move(_roamingConfigPath, _portableConfigPath);
        }

        if (!portable && PortableMode)
        {
            new FileInfo(_roamingConfigPath).Directory?.Create();
            File.Move(_portableConfigPath, _roamingConfigPath);
        }
    }

    public static Hotkeys? GetKeys(this Config config, HotkeysType type)
    {
        return config.Keys.TryGetValue(type, out var k) ? k : null;
    }

    public static Hotkeys SetKeys(this Config config, HotkeysType type, Hotkeys hotkeys)
    {
        var configKeys = config.GetKeys(type) ?? new Hotkeys();
        configKeys.ModifierKeys.Clear();
        foreach (var key in hotkeys.ModifierKeys)
        {
            configKeys.ModifierKeys.Add(key);
        }

        configKeys.Key = hotkeys.Key;
        config.Keys[type] = configKeys;
        return configKeys;
    }


    private static void UseConfig(Config c)
    {
        Config.ProfileName = c.ProfileName;
        Config.IsCurrent = c.IsCurrent;
        Config.DisableInFullScreen = c.DisableInFullScreen;
        Config.WindowSizes = c.WindowSizes;
        Config.CheckUpdate = c.CheckUpdate;

        Config.Keys.Clear();
        foreach (var key in c.Keys)
        {
            Config.SetKeys(key.Key, key.Value);
        }
    }


    #region migrate config(v1.1.0) to profiles(v1.2.0)

    private static void MigrateToProfile(string path)
    {
        LoadOldConfig(path);
        Config.ProfileName = DefaultProfileName;
        Config.IsCurrent = true;
        Profiles.Configs.Add(Config);
        Save();
    }

    private static void LoadOldConfig(string path)
    {
        var text = File.ReadAllText(path);
        var c = JsonConvert.DeserializeObject<Config>(text);
        if (c is null)
        {
            return;
        }

        Config.Keys.Clear();
        foreach (var key in c.Keys)
        {
            Config.SetKeys(key.Key, key.Value);
        }

        Config.DisableInFullScreen = c.DisableInFullScreen;
        Config.WindowSizes = c.WindowSizes;
        Config.CheckUpdate = c.CheckUpdate;

        Config.Migrate(c);

        if (!Config.WindowSizes.Any())
        {
            return;
        }

        var sortedInstance = new BindingList<WindowSize>(
            Config.WindowSizes
                  .OrderBy(w => w.Name)
                  .ThenBy(w => w.Title)
                  .ToList()
        );
        Config.WindowSizes = sortedInstance;
    }

    #endregion
}
