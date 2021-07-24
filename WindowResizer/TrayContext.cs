using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Properties;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();

        private static SettingForm _settingForm;

        public TrayContext()
        {
            try
            {
                ConfigLoader.Load();
            }
            catch (Exception e)
            {
                MessageBox.Show($"WindowResizer: config load failed! Exception: {e.Message}");
            }

            try
            {
                RegisterHotkey();
            }
            catch (Exception e)
            {
                MessageBox.Show($"WindowResizer: register hotkey failed! Exception: {e.Message}");
            }

            _trayIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon,
                ContextMenu =
                    new ContextMenu(new MenuItem[] {new MenuItem("Setting", OnSetting), new MenuItem("Exit", OnExit)}),
                Visible = true,
                Text = "WindowResizer"
            };

            _trayIcon.DoubleClick += OnSetting;
        }

        private void OnExit(object sender, EventArgs e)
        {
            _settingForm?.Close();
            ConfigLoader.Save();
            _trayIcon.Dispose();
            _hook.Dispose();
            Environment.Exit(0);
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                _settingForm = new SettingForm(_hook);
            }

            _settingForm.Show();
        }

        private void RegisterHotkey()
        {
            if (!ConfigLoader.config.SaveKey.ValidateKeys())
            {
                MessageBox.Show("Save window hotkeys not valid.");
            }

            if (!ConfigLoader.config.RestoreKey.ValidateKeys())
            {
                MessageBox.Show("Restore window hotkeys not valid.");
            }

            _hook.RegisterHotKey(ConfigLoader.config.SaveKey.GetModifierKeys(), ConfigLoader.config.SaveKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.config.RestoreKey.GetModifierKeys(),
                ConfigLoader.config.RestoreKey.GetKey());
            _hook.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (ConfigLoader.config.DisbaleInFullScreen && WindowControl.IsForegroundFullScreen())
            {
                return;
            }

            var handle = WindowControl.GetForegroundHandle();
            var process = WindowControl.GetRealProcess(handle);
            if (ConfigLoader.config.WindowSizes == null)
            {
                ConfigLoader.config.WindowSizes = new BindingList<WindowSize>();
            }

            var processName = process.MainModule?.ModuleName;
            var title = process.MainWindowTitle;

            var match = GetMatchWindowSize(ConfigLoader.config.WindowSizes, processName, title);

            if (e.Modifier == ConfigLoader.config.SaveKey.GetModifierKeys() &&
                e.Key == ConfigLoader.config.SaveKey.GetKey())
            {
                UpdateOrSaveConfig(match, processName, title, WindowControl.GetRect(handle));
            }
            else
            {
                if (match.NoMatch)
                {
                    MessageBox.Show($"No saved settings for {processName}:{title}", "WindowResizer");
                }
                else
                {
                    MoveMatchWindow(match, handle);
                }
            }
        }

        private static MatchWindowSize GetMatchWindowSize(BindingList<WindowSize> windowSizes, string processName,
            string title)
        {
            var windows = windowSizes.Where(w => w.Name == processName).ToList();
            return new MatchWindowSize
            {
                FullMatch = windows.FirstOrDefault(w => w.Title == title),
                PrefixMatch = windows.FirstOrDefault(w =>
                    w.Title.StartsWith("*") && w.Title.Length > 1 && title.EndsWith(w.Title.TrimStart('*'))),
                SuffixMatch = windows.FirstOrDefault(w =>
                    w.Title.EndsWith("*") && w.Title.Length > 1 && title.StartsWith(w.Title.TrimEnd('*'))),
                WildcardMatch = windows.FirstOrDefault(w => w.Title.Equals("*"))
            };
        }

        private static void MoveMatchWindow(MatchWindowSize match, IntPtr handle)
        {
            if (match.FullMatch != null)
            {
                WindowControl.MoveWindow(handle, match.FullMatch.Rect);
                return;
            }

            if (match.PrefixMatch != null)
            {
                WindowControl.MoveWindow(handle, match.PrefixMatch.Rect);
                return;
            }

            if (match.SuffixMatch != null)
            {
                WindowControl.MoveWindow(handle, match.SuffixMatch.Rect);
                return;
            }

            if (match.WildcardMatch != null)
            {
                WindowControl.MoveWindow(handle, match.WildcardMatch.Rect);
            }
        }

        private static void UpdateOrSaveConfig(MatchWindowSize match, string processName, string title, Rect rect)
        {
            if (string.IsNullOrWhiteSpace(processName) || string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            if (match.NoMatch)
            {
                // Add a wildcard match for all titles
                InsertOrder(new WindowSize {Name = processName, Title = "*", Rect = rect});
                InsertOrder(new WindowSize {Name = processName, Title = title, Rect = rect});
                ConfigLoader.Save();
                return;
            }

            if (match.FullMatch != null)
            {
                match.FullMatch.Rect = rect;
            }
            else
            {
                InsertOrder(new WindowSize {Name = processName, Title = title, Rect = rect});
            }

            if (match.SuffixMatch != null)
            {
                match.SuffixMatch.Rect = rect;
            }

            if (match.PrefixMatch != null)
            {
                match.PrefixMatch.Rect = rect;
            }

            if (match.WildcardMatch != null)
            {
                match.WildcardMatch.Rect = rect;
            }
            else
            {
                InsertOrder(new WindowSize {Name = processName, Title = "*", Rect = rect});
            }

            ConfigLoader.Save();
        }

        private static void InsertOrder(WindowSize item)
        {
            var list = ConfigLoader.config.WindowSizes;
            var backing = list.ToList();
            backing.Add(item);
            var index = backing.OrderBy(l => l.Name).ThenBy(l => l.Title).ToList().IndexOf(item);
            list.Insert(index, item);
        }
    }
}
