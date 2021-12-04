using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowResizer
{
    public static class WindowControl
    {
        #region api

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            SWP_NOOWNERZORDER = 0x0200
        }

        [Flags]
        public enum ShowWindowFlags : uint
        {
            SW_SHOWNORMAL = 1
        }

        #endregion

        /// <summary>
        ///    Get all open windows
        /// </summary>
        /// <returns></returns>
        /// <remarks>via https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows/43640787#43640787</remarks>
        public static List<IntPtr> GetOpenWindows()
        {
            var shellWindow = GetShellWindow();
            var windows = new List<IntPtr>();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                var length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                windows.Add(hWnd);
                return true;
            }, 0);

            return windows;
        }

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
            if (handle == IntPtr.Zero)
                return;

            ShowWindow(handle, (int)ShowWindowFlags.SW_SHOWNORMAL);
            SetWindowPos(handle, 0, rect.Left, rect.Top,
                rect.Right - rect.Left, rect.Bottom - rect.Top,
                (int)SetWindowPosFlags.SWP_NOOWNERZORDER);
        }

        public static string GetProcessName(IntPtr handle)
        {
            try
            {
                uint pid;
                GetWindowThreadProcessId(handle, out pid);
                var proc = Process.GetProcessById((int)pid);
                return proc.MainModule?.ModuleName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetRealProcessName(IntPtr handle)
        {
            try
            {
                var proc = GetRealProcess(handle);
                return proc.MainModule?.ModuleName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Process GetRealProcess(IntPtr handle)
        {
            uint pid;
            GetWindowThreadProcessId(handle, out pid);
            var foregroundProcess = Process.GetProcessById((int)pid);
            if (foregroundProcess.ProcessName == "ApplicationFrameHost")
            {
                foregroundProcess = GetRealProcess(foregroundProcess);
            }

            return foregroundProcess;
        }

        private static Process _realProcess;

        private static Process GetRealProcess(Process foregroundProcess)
        {
            EnumChildWindows(foregroundProcess.MainWindowHandle, ChildWindowCallback, IntPtr.Zero);
            return _realProcess;
        }

        private static bool ChildWindowCallback(IntPtr handle, IntPtr lparam)
        {
            uint pid;
            GetWindowThreadProcessId(handle, out pid);
            var process = Process.GetProcessById((int)pid);
            if (process.ProcessName != "ApplicationFrameHost")
            {
                _realProcess = process;
            }

            return true;
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
