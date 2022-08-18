using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WindowResizer.Configuration
{
    public static class ConfigLoader
    {
        private const string ConfigFile = "WindowResizer.config.json";

        private static readonly string RoamingPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowResizer");

        private static string _roamingConfigPath = Path.Combine(RoamingPath, ConfigFile);

        private static string _portableConfigPath = string.Empty;

        public static bool PortableMode => File.Exists(RoamingPath);

        public static void SetPath(string roamingPath, string portablePath)
        {
            _roamingConfigPath = roamingPath;
            _portableConfigPath = portablePath;
        }

        public static string ConfigPath => PortableMode ? _portableConfigPath : _roamingConfigPath;

        public static Config Config = new();

        public static void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                Config.SaveKey = Config.DefaultSaveKey;
                Config.RestoreKey = Config.DefaultRestoreKey;
                Config.RestoreAllKey = Config.DefaultRestoreAllKey;
                Save();
                return;
            }

            var text = File.ReadAllText(ConfigPath);
            var c = JsonConvert.DeserializeObject<Config>(text);
            if (c != null)
            {
                Config = c;
            }

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

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Config);
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
    }
}
