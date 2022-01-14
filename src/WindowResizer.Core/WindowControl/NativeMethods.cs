using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowResizer.Core.WindowControl
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        internal delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        internal static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        internal delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [Flags]
        internal enum SetWindowPosFlags : uint
        {
            SWP_NOOWNERZORDER = 0x0200,
        }

        [Flags]
        internal enum ShowWindowFlags : uint
        {
            SW_SHOWNORMAL = 1,
            SW_SHOWMAXIMIZED = 3,
        }

        [Flags]
        internal enum WindowStyles : uint
        {
            WS_MINIMIZE = 0x20000000,
            WS_MAXIMIZE = 0x1000000,
        }
    }
}
