using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowResizer.Common.Windows;

namespace WindowResizer.Core.WindowControl;

internal static class NativeMethods
{
    [DllImport("user32.dll")]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement placement);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement placement);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    internal static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

    internal delegate bool WindowEnumProc(IntPtr hWnd, IntPtr lparam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowRect(IntPtr hWnd, ref Rect rectangle);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EnumChildWindows(IntPtr hWnd, WindowEnumProc callback, IntPtr lParam);

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
    internal enum WindowStyles : uint
    {
        WS_MINIMIZE = 0x20000000,
        WS_MAXIMIZE = 0x1000000,
    }

    internal enum ShowWindowCommands
    {
        Hide = 0,
        Normal = 1,
        ShowMinimized = 2,
        ShowMaximized = 3,
        ShowNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNA = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimize = 11
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        public int Length;
        public int Flags;
        public ShowWindowCommands ShowCmd;
        public Point MinPosition;
        public Point MaxPosition;
        public Rect NormalPosition;

        public static WindowPlacement Default
        {
            get
            {
                var result = new WindowPlacement();
                result.Length = Marshal.SizeOf(result);
                return result;
            }
        }
    }
}
