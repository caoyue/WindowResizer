using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowResizer
{
    public class Config
    {
        public string[] ModifierKeys { get; set; } = { "ctrl", "alt" };
        public string Key { get; set; } = "k";

        // set left or top to -1 means center
        public int Left { get; set; } = -1;
        public int Top { get; set; } = -1;
        public int Width { get; set; } = 1280;
        public int Height { get; set; } = 720;
    }

    public static class ConfigHelper
    {
        public static ModifierKeys GetModifierKeys(this Config config) {
            ModifierKeys keys = 0;
            foreach (var k in config.ModifierKeys) {
                ModifierKeys m;
                if (!Enum.TryParse(k, true, out m))
                    continue;

                keys = keys | m;
            }
            return keys;
        }

        public static Keys GetKey(this Config config) {
            Keys k;
            return Enum.TryParse(config.Key, true, out k) ? k : new Config().GetKey();
        }

    }

    public static class ConfigLoader
    {
        public static Config Load() {
            var path = Path.Combine(Application.StartupPath, "config.json");
            if (!File.Exists(path)) {
                var config = new Config();
                var json = JsonConvert.SerializeObject(config);
                File.WriteAllText(path, json);
                return config;
            }

            var text = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Config>(text);
        }
    }
}
