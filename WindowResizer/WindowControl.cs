using System;
using System.Runtime.InteropServices;
using System.Text;

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

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            var handle = GetForegroundWindow();

            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        public static void MoveWindow()
        {
            var handle = GetForegroundWindow();
            if (handle != IntPtr.Zero) {
                SetWindowPos(handle, 0, 0, 0, 600, 400, 0x0200);
            }
        }
    }
}
