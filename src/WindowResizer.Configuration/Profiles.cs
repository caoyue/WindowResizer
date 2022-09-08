using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WindowResizer.Configuration;

public class Profiles
{
    public List<Config> Configs { get; private set; } = new();

    [JsonIgnore]
    public const string DefaultProfileName = "default";

    [JsonIgnore]
    public Config Current => GetCurrentConfig();

    public void Rename(string name)
    {
        Current.ProfileName = name;
    }

    public Profiles UseDefaultProfile()
    {
        Configs.Clear();
        var defaultConfig = Config.DefaultConfig();
        Configs.Add(defaultConfig);
        return this;
    }

    private Config GetCurrentConfig()
    {
        if (Configs.Exists(i => i.IsCurrent))
        {
            return Configs.First();
        }

        var first = Configs.FirstOrDefault();
        if (first is null)
        {
            UseDefaultProfile();
            return Configs.First();
        }

        first.IsCurrent = true;
        return first;
    }
}
