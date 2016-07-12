using System;
using System.Windows.Forms;
using WindowResizer.Properties;

namespace WindowResizer {
    public class TrayContext : ApplicationContext {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();

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
            _hook.KeyPressed += OnKeyPressed;
            _hook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.K);
        }

        void OnKeyPressed(object sender, KeyPressedEventArgs e) {
            WindowControl.MoveWindow();
        }
    }
}
