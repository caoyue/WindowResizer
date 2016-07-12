using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowResizer
{
    public static class WindowControl
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        public static IntPtr GetForegroundHandle() {
            return GetForegroundWindow();
        }

        public static string GetActiveWindowTitle(IntPtr handle) {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        public static void MoveWindow(IntPtr handle, int left, int top, int width, int height) {
            if (handle == IntPtr.Zero) return;

            if (left == -1 || top == -1) {
                // compatible with multi screens
                var scr = Screen.FromHandle(handle).Bounds;
                left = (scr.Width - width) / 2;
                top = (scr.Height - height) / 2;
            }
            SetWindowPos(handle, 0, left, top, width, height, 0x0200);
        }

        public static string GetProcessPath(IntPtr handle) {
            try {
                uint pid;
                GetWindowThreadProcessId(handle, out pid);
                var proc = Process.GetProcessById((int)pid);
                return proc.MainModule.FileName;
            }
            catch (Exception) {
                return string.Empty;
            }
        }
    }
}
