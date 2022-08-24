using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WindowResizer.Common.Windows;
using static WindowResizer.Core.WindowControl.NativeMethods;

namespace WindowResizer.Core.WindowControl;

public static class Resizer
{
    /// <summary>
    ///    Get all open windows
    /// </summary>
    /// <returns></returns>
    public static List<IntPtr> GetOpenWindows()
    {
        var shellWindow = GetShellWindow();
        var windows = new List<IntPtr>();

        EnumWindows(delegate(IntPtr hWnd, int _)
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

    public static bool IsChildWindow(IntPtr handle)
    {
        var r = GetParent(handle);
        return r != IntPtr.Zero;
    }

    public static WindowState GetWindowState(IntPtr handle)
    {
        const int GWL_STYLE = -16;
        var style = (long)GetWindowLongPtr(handle, GWL_STYLE);
        if ((style & (int)WindowStyles.WS_MAXIMIZE) == (int)WindowStyles.WS_MAXIMIZE)
        {
            return WindowState.Maximized;
        }

        return (style & (int)WindowStyles.WS_MINIMIZE) == (int)WindowStyles.WS_MINIMIZE
            ? WindowState.Minimized
            : WindowState.Normal;
    }

    public static bool IsWindowVisible(IntPtr handle)
    {
        return NativeMethods.IsWindowVisible(handle);
    }

    public static string? GetActiveWindowTitle(IntPtr handle)
    {
        const int nChars = 256;
        var buff = new StringBuilder(nChars);
        return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
    }

    public static void MaximizeWindow(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;
        ShowWindow(handle, (int)ShowWindowFlags.SW_SHOWMAXIMIZED);
    }

    public static bool MoveWindow(IntPtr handle, Rect rect)
    {
        if (handle == IntPtr.Zero)
            return false;

        ShowWindow(handle, (int)ShowWindowFlags.SW_SHOWNORMAL);
        var result = SetWindowPos(handle, 0, rect.Left, rect.Top,
            rect.Right - rect.Left, rect.Bottom - rect.Top,
            (int)SetWindowPosFlags.SWP_NOOWNERZORDER);

        return result == IntPtr.Zero;
    }

    public static string? GetProcessName(IntPtr handle)
    {
        try
        {
            _ = GetWindowThreadProcessId(handle, out var pid);
            var proc = Process.GetProcessById((int)pid);
            return proc.MainModule?.ModuleName;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string? GetRealProcessName(IntPtr handle)
    {
        try
        {
            var proc = GetRealProcess(handle);
            return proc?.MainModule?.ModuleName;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static Process? GetRealProcess(IntPtr handle)
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

    private static Process? _realProcess;

    private static Process? GetRealProcess(Process foregroundProcess)
    {
        EnumChildWindows(foregroundProcess.MainWindowHandle, ChildWindowCallback, IntPtr.Zero);
        return _realProcess;
    }

    private static bool ChildWindowCallback(IntPtr handle, IntPtr lparam)
    {
        _ = GetWindowThreadProcessId(handle, out var pid);
        var process = GetProcess(pid);
        if (process != null && process.ProcessName != "ApplicationFrameHost")
        {
            _realProcess = process;
        }

        return true;
    }

    private static Process? GetProcess(uint pid)
    {
        try
        {
            return Process.GetProcessById((int)pid);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static Rect GetRect(IntPtr handle)
    {
        var rect = new Rect();
        GetWindowRect(handle, ref rect);
        return rect;
    }

    public static bool IsForegroundFullScreen(Screen? screen = null)
    {
        screen ??= Screen.PrimaryScreen;

        var rect = GetRect(GetForegroundWindow());
        return screen.Bounds.Width == rect.Right - rect.Left
            && screen.Bounds.Height == rect.Bottom - rect.Top;
    }

    public static bool IsInvisibleProcess(string processName)
    {
        return InvisibleProcesses.Contains(processName.ToUpper());
    }

    private static HashSet<string> InvisibleProcesses => new()
    {
        "TEXTINPUTHOST.EXE"
    };
}