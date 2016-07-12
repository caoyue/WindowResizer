using System;
using System.Windows.Forms;
using WindowResizer.Properties;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();
        private readonly Config config = ConfigLoader.Load();

        public TrayContext() {
            try {
                RegisterHotkey();
            }
            catch (Exception e) {
                MessageBox.Show($"WindowResizer: register hotkey failed! Exception: {e.Message}");
            }

            _trayIcon = new NotifyIcon {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
                Text = "WindowResizer"
            };
        }

        private void Exit(object sender, EventArgs e) {
            _trayIcon.Visible = false;
            _hook.Dispose();
            Application.Exit();
        }

        private void RegisterHotkey() {
            _hook.RegisterHotKey(config.GetModifierKeys(), config.GetKey());
            _hook.KeyPressed += OnKeyPressed;
        }

        void OnKeyPressed(object sender, KeyPressedEventArgs e) {
            var handle = WindowControl.GetForegroundHandle();
            WindowControl.MoveWindow(handle, config.Left, config.Top, config.Width, config.Height);
        }
    }
}
