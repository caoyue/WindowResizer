using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Properties;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();
        private readonly Config _config = ConfigLoader.Load();

        private static SettingForm _settingForm;

        public TrayContext()
        {
            try
            {
                RegisterHotkey(_config);
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
            ConfigLoader.Save(_config);
            _trayIcon.Visible = false;
            _hook.Dispose();
            Application.Exit();
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                _settingForm = new SettingForm(_config, _hook);
            }

            _settingForm.Show();
            _settingForm.BringToFront();
        }

        private void RegisterHotkey(Config config)
        {
            if (!config.SaveKey.ValidateKeys())
            {
                MessageBox.Show("Save window hotkeys not valid.");
            }
            if (!config.RestoreKey.ValidateKeys())
            {
                MessageBox.Show("Restore window hotkeys not valid.");
            }
            _hook.RegisterHotKey(config.SaveKey.GetModifierKeys(), config.SaveKey.GetKey());
            _hook.RegisterHotKey(config.RestoreKey.GetModifierKeys(), config.RestoreKey.GetKey());
            _hook.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (_config.DisbaleInFullScreen && WindowControl.IsForegroundFullScreen())
            {
                return;
            }

            var handle = WindowControl.GetForegroundHandle();
            var process = WindowControl.GetRealProcessName(handle);
            if (_config.WindowSizes == null)
            {
                _config.WindowSizes = new List<WindowSize>();
            }
            var windowSize = _config.WindowSizes.FirstOrDefault(w => w.Process == process);


            if (e.Modifier == _config.SaveKey.GetModifierKeys() && e.Key == _config.SaveKey.GetKey())
            {
                var rect = WindowControl.GetRect(handle);
                if (windowSize == null)
                {
                    _config.WindowSizes.Add(new WindowSize
                    {
                        Process = process,
                        Rect = rect
                    });
                }
                else
                {
                    windowSize.Rect = WindowControl.GetRect(handle);
                }
                ConfigLoader.Save(_config);
            }
            else
            {
                var rect = windowSize?.Rect ?? new Rect
                {
                    Top = 0,
                    Bottom = 720,
                    Left = 0,
                    Right = 1280
                };
                WindowControl.MoveWindow(handle, rect);
            }
        }
    }
}
