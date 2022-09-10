using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WindowResizer.Common.Shortcuts;

namespace WindowResizer.Configuration;

public static class ConfigFactory
{
    private const string ConfigFile = $"{nameof(WindowResizer)}.config.json";

    private static readonly string RoamingPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer));

    private static string _roamingConfigPath = Path.Combine(RoamingPath, ConfigFile);
    private static string _portableConfigPath = string.Empty;

    public static bool PortableMode => File.Exists(RoamingPath);

    public static void SetPath(string roamingPath, string portablePath)
    {
        _roamingConfigPath = roamingPath;
        _portableConfigPath = portablePath;
    }

    public static string ConfigPath => PortableMode ? _portableConfigPath : _roamingConfigPath;

    public static readonly Profiles Profiles = new();

    public static Config Current => GetCurrentConfig();

    #region Config

    public static void Load()
    {
        if (!File.Exists(ConfigPath))
        {
            Save();
            return;
        }

        Load(ConfigPath);
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

    #endregion

    #region Profiles

    public static void UseDefault() =>
        Profiles.UseDefault();

    public static Config ProfileAdd(string profileName)
    {
        var p = Profiles.Add(profileName);
        Save();
        return p;
    }

    public static void ProfileRename(string profileId, string profileName)
    {
        Profiles.Rename(profileId, profileName);
        Save();
    }

    public static bool ProfileRemove(string profileId)
    {
        if (!Profiles.Remove(profileId))
        {
            return false;
        }

        Save();
        return true;
    }

    public static bool ProfileSwitch(string profileId)
    {
        if (!Profiles.Switch(profileId))
        {
            return false;
        }

        Save();
        return true;
    }

    #endregion

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

    private static Config GetCurrentConfig()
    {
        var cur = Profiles.Current;
        if (cur is not null)
        {
            return cur;
        }

        var f = Profiles.Configs.FirstOrDefault();
        if (f is null)
        {
            return Profiles.UseDefault();
        }

        Profiles.Switch(f.ProfileId);
        return f;
    }

    #region migrate config(v1.1.0) to profiles(v1.2.0)

    private static void MigrateToProfile(string path)
    {
        Profiles.Configs.Clear();

        var config = LoadOldConfig(path);
        if (config is not null)
        {
            config.ProfileName = Profiles.DefaultProfileName;
            config.ProfileId = Config.GenerateConfigId();
            Profiles.Configs.Add(config);
            Profiles.Switch(config.ProfileId);
            Save();
        }
    }

    private static Config? LoadOldConfig(string path)
    {
        var text = File.ReadAllText(path);
        var c = JsonConvert.DeserializeObject<Config>(text);
        if (c is null)
        {
            return null;
        }

        var config = new Config();
        foreach (var key in c.Keys)
        {
            config.SetKeys(key.Key, key.Value);
        }

        config.DisableInFullScreen = c.DisableInFullScreen;
        config.WindowSizes = c.WindowSizes;
        config.CheckUpdate = c.CheckUpdate;

        config.Migrate(c);

        if (!config.WindowSizes.Any())
        {
            return config;
        }

        var sortedInstance = new BindingList<WindowSize>(
            config.WindowSizes
                .OrderBy(w => w.Name)
                .ThenBy(w => w.Title)
                .ToList()
        );
        config.WindowSizes = sortedInstance;
        return config;
    }

    #endregion
}
