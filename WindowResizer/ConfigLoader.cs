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

        /// <summary>
        ///     Window State
        /// </summary>
        public WindowState State { get; set; } = WindowState.Normal;

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
            get { return File.Exists(_roamingPath); }
        }

        public static string ConfigPath
        {
            get { return PortableMode ? _portableConfigPath : _roamingConfigPath; }
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
