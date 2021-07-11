using System;
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

            var name = process.MainModule.ModuleName;
            var title = process.MainWindowTitle;
            WindowSize windowSize;
            var windowSizes = ConfigLoader.config.WindowSizes.Where(w => w.Name == name).ToList();

            // full match
            var fullMatch = windowSizes.FirstOrDefault(w => w.Title == title);
            if (fullMatch != null)
            {
                windowSize = fullMatch;
            }
            else
            {
                // prefix match (*title)
                var prefixMatch = windowSizes.FirstOrDefault(w => w.Title.StartsWith("*") &&
                                                                  title.EndsWith(w.Title.TrimStart('*')));
                if (prefixMatch != null)
                {
                    windowSize = prefixMatch;
                }
                else
                {
                    // suffix match (title*)
                    var suffixMatch = windowSizes.FirstOrDefault(w =>
                        w.Title.EndsWith("*") && title.StartsWith(w.Title.TrimEnd('*')));
                    if (suffixMatch != null)
                    {
                        windowSize = suffixMatch;
                    }
                    else
                    {
                        // wildcard match (*)
                        var wildcardMatch = windowSizes.FirstOrDefault(w => w.Title == "*");
                        windowSize = wildcardMatch;
                    }
                }
            }

            if (e.Modifier == ConfigLoader.config.SaveKey.GetModifierKeys() &&
                e.Key == ConfigLoader.config.SaveKey.GetKey())
            {
                var rect = WindowControl.GetRect(handle);
                if (windowSize != null)
                {
                    if (windowSize.Title == "*")
                    {
                        ConfigLoader.config.WindowSizes.Add(new WindowSize
                        {
                            Name = process.MainModule.ModuleName, Title = process.MainWindowTitle, Rect = rect
                        });
                    }
                    else
                    {
                        windowSize.Rect = WindowControl.GetRect(handle);
                    }
                }
                else
                {
                    ConfigLoader.config.WindowSizes.Add(new WindowSize
                    {
                        Name = process.MainModule.ModuleName, Title = process.MainWindowTitle, Rect = rect
                    });

                    // Add a wildcard match for all titles
                    ConfigLoader.config.WindowSizes.Add(new WindowSize
                    {
                        Name = process.MainModule.ModuleName, Title = "*", Rect = rect
                    });
                }

                ConfigLoader.Save();
            }
            else
            {
                if (windowSize == null)
                {
                    MessageBox.Show(
                        "No saved settings for " + process.MainModule.ModuleName + ":" + process.MainWindowTitle,
                        "WindowResizer");
                }
                else
                {
                    WindowControl.MoveWindow(handle, windowSize.Rect);
                }
            }
        }
    }
}
