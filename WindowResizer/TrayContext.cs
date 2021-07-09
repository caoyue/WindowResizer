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
            } catch (Exception e)
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
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Setting", OnSetting),
                    new MenuItem("Exit", OnExit)
                }),
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
            _hook.RegisterHotKey(ConfigLoader.config.RestoreKey.GetModifierKeys(), ConfigLoader.config.RestoreKey.GetKey());
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
            var windowSize = ConfigLoader.config.WindowSizes.FirstOrDefault(w =>
            {
                if (w.Title.StartsWith("*"))
                {
                    return w.Name == process.MainModule.ModuleName && process.MainWindowTitle.EndsWith(w.Title.Substring(1));
                }
                else if (w.Title.EndsWith("*"))
                {
                    return w.Name == process.MainModule.ModuleName && process.MainWindowTitle.StartsWith(w.Title.Substring(0, w.Title.Length - 1));
                }
                else
                {
                    return w.Name == process.MainModule.ModuleName && process.MainWindowTitle == w.Title;
                }
            });

            if (e.Modifier == ConfigLoader.config.SaveKey.GetModifierKeys() && e.Key == ConfigLoader.config.SaveKey.GetKey())
            {
                var rect = WindowControl.GetRect(handle);
                if (windowSize == null)
                {
                    ConfigLoader.config.WindowSizes.Add(new WindowSize
                    {
                        Name = process.MainModule.ModuleName,
                        Title = process.MainWindowTitle,
                        Rect = rect
                    });
                }
                else
                {
                    windowSize.Rect = WindowControl.GetRect(handle);
                }
                ConfigLoader.Save();
            }
            else
            {
                if (windowSize == null)
                {
                    MessageBox.Show("No saved settings for " + process.MainModule.ModuleName + ":" + process.MainWindowTitle, "WindowResizer");
                }
                else
                {
                    WindowControl.MoveWindow(handle, windowSize.Rect);
                }
            }
        }
    }
}
