﻿using System;
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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public static IntPtr GetForegroundHandle()
        {
            return GetForegroundWindow();
        }

        public static string GetActiveWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        public static void MoveWindow(IntPtr handle, Rect rect)
        {
            if (handle == IntPtr.Zero) return;

            SetWindowPos(handle, 0, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, 0x0200);
        }

        public static string GetProcessPath(IntPtr handle)
        {
            try
            {
                uint pid;
                GetWindowThreadProcessId(handle, out pid);
                var proc = Process.GetProcessById((int)pid);
                return proc.MainModule.FileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Rect GetRect(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            return rect;
        }


        public static bool IsForegroundFullScreen(Screen screen = null)
        {

            if (screen == null)
            {
                screen = Screen.PrimaryScreen;
            }

            var rect = GetRect(GetForegroundWindow());
            return screen.Bounds.Width == rect.Right - rect.Left
                   && screen.Bounds.Height == rect.Bottom - rect.Top;
        }
    }

    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }
}
