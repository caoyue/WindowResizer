using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WindowResizer.Common.Shortcuts;

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

        public static readonly Config Config = new();


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
            Config.Keys.Clear();
            foreach (var key in Config.DefaultKeys)
            {
                Config.Keys.Add(key.Key, key.Value);
            }


            Config.DisableInFullScreen = true;
            Config.WindowSizes.Clear();
            Config.CheckUpdate = true;
        }

        public static void Load(string path)
        {
            var text = File.ReadAllText(path);
            var c = JsonConvert.DeserializeObject<Config>(text);
            if (c is null)
            {
                return;
            }

            Config.Keys = c.Keys;
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

        public static Hotkeys? GetKeys(this Config config, HotkeysType type)
        {
            return config.Keys.TryGetValue(type, out var k) ? k : null;
        }

        public static Hotkeys SetKeys(this Config config, HotkeysType type, Hotkeys hotkeys)
        {
            var configKeys = config.GetKeys(type) ?? new Hotkeys();
            configKeys.ModifierKeys = hotkeys.ModifierKeys;
            configKeys.Key = hotkeys.Key;
            config.Keys[type] = configKeys;
            return configKeys;
        }
    }
}
