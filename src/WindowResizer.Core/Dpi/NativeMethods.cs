using System;
using System.Runtime.InteropServices;
using WindowResizer.Common.Windows;

namespace WindowResizer.Core.Dpi;

internal static class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

    [DllImport("SHCore.dll", SetLastError = true)]
    internal static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

    [DllImport("user32.dll")]
    internal static extern bool SetProcessDPIAware();

    [DllImport("SHcore.dll")]
    internal static extern int GetProcessDpiAwareness(IntPtr hWnd, out PROCESS_DPI_AWARENESS value);

    [DllImport("user32.dll")]
    internal static extern int GetDpiForWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern IntPtr SetThreadDpiAwarenessContext(IntPtr dpiContext);

    [DllImport("user32.dll")]
    internal static extern int GetAwarenessFromDpiAwarenessContext(IntPtr dpiContext);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool AreDpiAwarenessContextsEqual(IntPtr dpiContextA, IntPtr dpiContextB);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromRect([In] ref Rect rect, uint dwFlags);

    internal enum PROCESS_DPI_AWARENESS
    {
        Process_DPI_Unaware = 0,
        Process_System_DPI_Aware = 1,
        Process_Per_Monitor_DPI_Aware = 2
    }

    internal enum DPI_AWARENESS_CONTEXT
    {
        DPI_AWARENESS_CONTEXT_UNAWARE = 16,
        DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = 17,
        DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = 18,
        DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = 34
    }

    internal enum DPI_AWARENESS
    {
        DPI_AWARENESS_INVALID = -1,
        DPI_AWARENESS_UNAWARE = 0,
        DPI_AWARENESS_SYSTEM_AWARE = 1,
        DPI_AWARENESS_PER_MONITOR_AWARE = 2
    }

    internal enum MONITOR_FLAGS
    {
        MONITOR_DEFAULTTONULL = 0,
        MONITOR_DEFAULTTOPRIMARY = 1,
        MONITOR_DEFAULTTONEAREST = 2,
    }
}
