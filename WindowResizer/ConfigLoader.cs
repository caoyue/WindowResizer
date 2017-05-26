using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowResizer
{
    public class Config
    {
        public HotKeys SaveKey { get; set; } = new HotKeys()
        {
            ModifierKeys = new[] { "ctrl", "alt" },
            Key = "s"
        };

        public HotKeys RestoreKey { get; set; } = new HotKeys()
        {
            ModifierKeys = new[] { "ctrl", "alt" },
            Key = "r"
        };

        public List<WindowSize> WindowSizes { get; set; }
    }

    public class WindowSize
    {
        public string Process { get; set; }

        public Rect Rect { get; set; }
    }

    public class HotKeys
    {
        public string[] ModifierKeys { get; set; }
        public string Key { get; set; }
    }

    public static class ConfigHelper
    {
        public static ModifierKeys GetModifierKeys(this HotKeys hotKeys)
        {
            ModifierKeys keys = 0;
            foreach (var k in hotKeys.ModifierKeys)
            {
                ModifierKeys m;
                if (!Enum.TryParse(k, true, out m))
                    continue;

                keys = keys | m;
            }
            return keys;
        }

        public static Keys GetKey(this HotKeys hotKeys)
        {
            Keys k;
            return Enum.TryParse(hotKeys.Key, true, out k) ? k : new HotKeys().GetKey();
        }

    }

    public static class ConfigLoader
    {
        private static readonly string Path = System.IO.Path.Combine(Application.StartupPath, "config.json");

        public static Config Load()
        {
            if (!File.Exists(Path))
            {
                var config = new Config();
                Save(config);
                return config;
            }

            var text = File.ReadAllText(Path);
            return JsonConvert.DeserializeObject<Config>(text);
        }

        public static void Save(Config config)
        {
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(Path, json);
        }
    }
}
