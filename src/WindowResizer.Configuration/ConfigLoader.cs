using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowResizer.Configuration
{
    public static class ConfigLoader
    {
        private const string ConfigFile = "WindowResizer.config.json";

        private static readonly string RoamingPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowResizer");

        private static readonly string PortableConfigPath = Path.Combine(
            Application.StartupPath, ConfigFile);

        private static readonly string RoamingConfigPath = Path.Combine(
            RoamingPath, ConfigFile);

        public static bool PortableMode
        {
            get { return File.Exists(RoamingPath); }
        }

        public static string ConfigPath
        {
            get { return PortableMode ? PortableConfigPath : RoamingConfigPath; }
        }

        public static Config Config = new Config();

        public static void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                if (Config.WindowSizes == null)
                {
                    Config.WindowSizes = new BindingList<WindowSize>();
                }

                Save();
            }
            else
            {
                var text = File.ReadAllText(ConfigPath);
                var c  = JsonConvert.DeserializeObject<Config>(text);
                if (c != null)
                {
                    Config = c;
                }
                
                if (Config.WindowSizes.Any())
                {
                    var sortedInstance = new BindingList<WindowSize>(
                        Config.WindowSizes
                              .OrderBy(w => w.Name)
                              .ThenBy(w => w.Title)
                              .ToList()
                    );
                    Config.WindowSizes = sortedInstance;
                }
            }
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
                File.Move(RoamingConfigPath, PortableConfigPath);
            }

            if (!portable && PortableMode)
            {
                new FileInfo(RoamingConfigPath).Directory?.Create();
                File.Move(PortableConfigPath, RoamingConfigPath);
            }
        }
    }
}
