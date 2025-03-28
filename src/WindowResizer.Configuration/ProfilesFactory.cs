using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Common.Utils;

namespace WindowResizer.Configuration;

public static class ProfilesFactory
{
    private static string _roamingConfigPath = string.Empty;
    private static string _portableConfigPath = string.Empty;

    public static bool PortableMode;
    public static bool PostponeSaving = false;
    public static string ConfigPath = string.Empty;

    public static void SetPath(string roamingPath, string portablePath)
    {
        _roamingConfigPath = roamingPath;
        _portableConfigPath = portablePath;
        PortableMode = File.Exists(_portableConfigPath);
        ConfigPath = PortableMode ? _portableConfigPath : _roamingConfigPath;
    }

    public static readonly Profiles Profiles = new();

    public static ProfileConfig Current => GetCurrentProfile();

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
            throw new Exception("Could not load config.");
        }

        foreach (var config in p.Configs)
        {
            #region migrate: ensure config id

            if (config.WindowSizes.Any())
            {
                foreach (var ws in config.WindowSizes)
                {
                    if (string.IsNullOrEmpty(ws.WindowSizeId))
                    {
                        ws.WindowSizeId = ConfigHelper.GenerateConfigId();
                    }
                }
            }

            #endregion

            Profiles.Configs.Add(config);
        }

        Profiles.Switch(p.CurrentProfileId);
    }

    public static void Save()
    {
        var json = JsonConvert.SerializeObject(Profiles);
        new FileInfo(ConfigPath).Directory?.Create();

        if (!PostponeSaving) File.WriteAllText(ConfigPath, json);

        Profiles.Updated();
    }

    public static void Move(bool portable)
    {
        if (portable && !PortableMode)
        {
            File.Move(_roamingConfigPath, _portableConfigPath);
        }
        else if (!portable && PortableMode)
        {
            new FileInfo(_roamingConfigPath).Directory?.Create();
            File.Move(_portableConfigPath, _roamingConfigPath);
        }
    }

    #endregion

    #region Profiles

    public static void UseDefault() =>
        Profiles.UseDefault();

    public static ProfileConfig ProfileAdd(string profileName)
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

    public static Hotkeys? GetKeys(this ProfileConfig profileConfig, HotkeysType type)
    {
        return profileConfig.Keys.TryGetValue(type, out var k) ? k : null;
    }

    public static Hotkeys SetKeys(this ProfileConfig profileConfig, HotkeysType type, Hotkeys hotkeys)
    {
        var configKeys = profileConfig.GetKeys(type) ?? new Hotkeys();
        configKeys.ModifierKeys.Clear();
        foreach (var key in hotkeys.ModifierKeys)
        {
            configKeys.ModifierKeys.Add(key);
        }

        configKeys.Key = hotkeys.Key;
        profileConfig.Keys[type] = configKeys;
        return configKeys;
    }

    private static ProfileConfig GetCurrentProfile()
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
}
