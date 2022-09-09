using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WindowResizer.Configuration;

public class Profiles
{
    public string CurrentProfileId { get; private set; } = string.Empty;

    public List<Config> Configs { get; private set; } = new();

    [JsonIgnore]
    public const string DefaultProfileName = "default";

    [JsonIgnore]
    public Config? Current => Get(CurrentProfileId);

    public Config UseDefault()
    {
        Configs.Clear();
        var defaultConfig = Add(DefaultProfileName);
        CurrentProfileId = defaultConfig.ProfileId;
        return defaultConfig;
    }

    public bool Rename(string profileId, string profileName)
    {
        var profile = Get(profileId);
        if (profile is null)
        {
            return false;
        }

        profile.ProfileName = profileName;
        return true;
    }

    public Config Add(string profileName)
    {
        var newConfig = Config.NewConfig(profileName);
        Configs.Add(newConfig);
        return newConfig;
    }

    public bool Remove(string profileId)
    {
        var config = Get(profileId);
        if (config is null)
        {
            return false;
        }

        Configs.Remove(config);
        return true;
    }

    public delegate void ProfileSwitchEvent(Config config) ;

    public ProfileSwitchEvent? OnProfileSwitch;

    private Config? Get(string profileId) =>
        Configs.FirstOrDefault(i => i.ProfileId.Equals(profileId, StringComparison.Ordinal));

    public bool Switch(string profileId)
    {
        var p = Get(profileId);
        if (p is null)
        {
            return false;
        }

        CurrentProfileId = p.ProfileId;
        OnProfileSwitch?.Invoke(p);
        return true;
    }
}
