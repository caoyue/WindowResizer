using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowResizer
{
    public class Config
    {
        public bool DisableInFullScreen { get; set; } = true;

        public HotKeys SaveKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "S" };

        public HotKeys RestoreKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "R" };

        public HotKeys RestoreAllKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "T" };

        public BindingList<WindowSize> WindowSizes { get; set; }
    }

    public class WindowSize : IComparable<WindowSize>
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public Rect Rect { get; set; }

        public bool AutoResize { get; set; }

        //seperate these for the grid edit, but flag so they don't get JSON seriealized
        [JsonIgnore]
        public int Top
        {
            get { return Rect.Top; }
            set
            {
                Rect = new Rect
                {
                    Top = value,
                    Left = Rect.Left,
                    Right = Rect.Right,
                    Bottom = Rect.Bottom
                };
            }
        }

        [JsonIgnore]
        public int Left
        {
            get { return Rect.Left; }
            set
            {
                Rect = new Rect
                {
                    Top = Rect.Top,
                    Left = value,
                    Right = Rect.Right,
                    Bottom = Rect.Bottom
                };
            }
        }

        [JsonIgnore]
        public int Right
        {
            get { return Rect.Right; }
            set
            {
                Rect = new Rect
                {
                    Top = Rect.Top,
                    Left = Rect.Left,
                    Right = value,
                    Bottom = Rect.Bottom
                };
            }
        }

        [JsonIgnore]
        public int Bottom
        {
            get { return Rect.Bottom; }
            set
            {
                Rect = new Rect
                {
                    Top = Rect.Top,
                    Left = Rect.Left,
                    Right = Rect.Right,
                    Bottom = value
                };
            }
        }

        public int CompareTo(WindowSize other)
        {
            var c = string.Compare(other.Name, Name, StringComparison.Ordinal);
            return c == 0 ? string.Compare(other.Title, Title, StringComparison.Ordinal) : c;
        }
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

                keys |= m;
            }

            return keys;
        }

        public static Keys GetKey(this HotKeys hotKeys)
        {
            Keys k;
            return Enum.TryParse(hotKeys.Key, true, out k) ? k : new HotKeys().GetKey();
        }

        public static bool ValidateKeys(this HotKeys hotKeys)
        {
            return !(hotKeys.ModifierKeys == null ||
                hotKeys.ModifierKeys.Length == 0 ||
                string.IsNullOrEmpty(hotKeys.Key));
        }

        public static string ToKeysString(this HotKeys hotKeys)
        {
            string str = string.Empty;
            if (hotKeys.ModifierKeys != null && hotKeys.ModifierKeys.Length > 0)
            {
                str += string.Join(" + ", hotKeys.ModifierKeys);
            }

            return $"{str} + {hotKeys.Key}";
        }
    }

    public static class ConfigLoader
    {
        private const string ConfigFile = "WindowResizer.config.json";

        private static readonly string _roamingPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowResizer");

        private static readonly string _portableConfigPath = Path.Combine(
            Application.StartupPath, ConfigFile);

        private static readonly string _roamingConfigPath = Path.Combine(
            _roamingPath, ConfigFile);

        public static bool PortableMode
        {
            get { return !File.Exists(_roamingConfigPath); }
        }

        public static string ConfigPath
        {
            get { return PortableMode ? _portableConfigPath : _roamingConfigPath; }
        }

        public static Config Config = new Config();

        public static void Load()
        {
            // migration
            ConfigMigrationV2();
            ConfigMigrationV1();

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
                Config = JsonConvert.DeserializeObject<Config>(text);

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

        #region config migration

        [Obsolete]
        public class ConfigOld
        {
            public bool DisbaleInFullScreen { get; set; } = true;

            public HotKeys SaveKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "S" };

            public HotKeys RestoreKey { get; set; } = new HotKeys() { ModifierKeys = new[] { "Ctrl", "Alt" }, Key = "R" };

            public List<WindowSizeOldCfg> WindowSizes { get; set; }
        }

        [Obsolete]
        public class WindowSizeOldCfg
        {
            public string Process { get; set; }
            public Rect Rect { get; set; }
        }

        private static readonly string _oldPath = Path.Combine(Application.StartupPath, "config.json");

        private static void ConfigMigrationV1()
        {
            if (File.Exists(ConfigPath) || !File.Exists(_oldPath))
            {
                return;
            }

            var text = File.ReadAllText(_oldPath);
            ConfigOld configOld = JsonConvert.DeserializeObject<ConfigOld>(text);
            Config.DisableInFullScreen = configOld.DisbaleInFullScreen;
            Config.RestoreKey = configOld.RestoreKey;
            Config.SaveKey = configOld.SaveKey;
            Config.WindowSizes = new BindingList<WindowSize>();
            foreach (var w in configOld.WindowSizes)
            {
                Config.WindowSizes.Add(new WindowSize
                {
                    Name = w.Process,
                    Title = "*",
                    Rect = new Rect
                    {
                        Top = w.Rect.Top,
                        Left = w.Rect.Left,
                        Right = w.Rect.Right,
                        Bottom = w.Rect.Bottom
                    }
                });
            }

            File.Move(_oldPath, $"{_oldPath}.bak");
        }

        private static void ConfigMigrationV2()
        {
            if (File.Exists(_roamingConfigPath) || File.Exists(_portableConfigPath))
            {
                return;
            }

            try
            {
                var directoryInfo = new DirectoryInfo(_roamingPath);
                var files = directoryInfo
                            .GetFiles("config.json", SearchOption.AllDirectories)
                            .OrderByDescending(f => f.LastWriteTime)
                            .ToList();

                // copy file
                var lastConfig = files.FirstOrDefault();
                if (lastConfig != null)
                {
                    var text = FixTypo(File.ReadAllText(lastConfig.FullName));
                    File.WriteAllText(ConfigPath, text);
                }

                // clean up
                foreach (var file in files)
                {
                    File.Move(file.FullName, $"{file.FullName}.bak");
                }
            }
            catch
            {
                // ignored
            }
        }

        private static string FixTypo(string config)
        {
            return config.Replace("DisbaleInFullScreen", "DisableInFullScreen");
        }

        #endregion
    }
}
