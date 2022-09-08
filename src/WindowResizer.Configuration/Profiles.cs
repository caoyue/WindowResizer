using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WindowResizer.Configuration;

public class Profiles
{
    public List<Config> Configs { get; private set; } = new();

    [JsonIgnore]
    public Config Current => GetCurrentConfig();

    private Config GetCurrentConfig()
    {
        if (Configs.Exists(i => i.IsCurrent))
        {
            return Configs.First();
        }

        var first = Configs.FirstOrDefault();
        if (first is null)
        {
            return new Config
            {
                ProfileName = "default", IsCurrent = true,
            };
        }

        first.IsCurrent = true;
        return first;
    }
}
