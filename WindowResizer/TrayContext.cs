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

        public TrayContext()
        {
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
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
                Text = "WindowResizer"
            };
        }

        private void Exit(object sender, EventArgs e)
        {
            ConfigLoader.Save(_config);
            _trayIcon.Visible = false;
            _hook.Dispose();
            Application.Exit();
        }

        private void RegisterHotkey()
        {
            _hook.RegisterHotKey(_config.SaveKey.GetModifierKeys(), _config.SaveKey.GetKey());
            _hook.RegisterHotKey(_config.RestoreKey.GetModifierKeys(), _config.RestoreKey.GetKey());
            _hook.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
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
