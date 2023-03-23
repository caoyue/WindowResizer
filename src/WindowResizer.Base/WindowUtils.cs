using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Common.Windows;
using WindowResizer.Configuration;
using WindowResizer.Core.WindowControl;

namespace WindowResizer.Base;

public static class WindowUtils
{
    /// <summary>
    ///     Resize window
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="config"></param>
    /// <param name="onFailed"></param>
    /// <param name="onConfigNoMatch"></param>
    /// <param name="onlyAuto"></param>
    public static void ResizeWindow(
        IntPtr handle,
        Config config,
        Action<Process, Exception>? onFailed,
        Action<string, string>? onConfigNoMatch,
        bool onlyAuto = false)
    {
        if (!IsProcessAvailable(handle, out string processName, onFailed))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(processName)) return;

        var windowTitle = Resizer.GetWindowTitle(handle) ?? string.Empty;
        var match = GetMatchWindowSize(config.WindowSizes, processName, windowTitle, onlyAuto);
        if (!match.NoMatch)
        {
            MoveMatchWindow(match, handle);
        }
        else
        {
            onConfigNoMatch?.Invoke(processName, windowTitle);
        }
    }

    public static bool ResizeAllWindow(Config profile, Action<string>? onError)
    {
        var windows = Resizer.GetOpenWindows();
        windows.Reverse();
        foreach (var window in windows)
        {
            if (!profile.RestoreAllIncludeMinimized && Resizer.GetWindowState(window) == WindowState.Minimized)
            {
                continue;
            }

            ResizeWindow(window, profile, null, null);
        }

        return true;
    }

    /// <summary>
    ///     Update or save window size
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="config"></param>
    /// <param name="onFailed"></param>
    public static void UpdateOrSaveWindowSize(
        IntPtr handle,
        Config config,
        Action<Process, Exception>? onFailed)
    {
        if (!IsProcessAvailable(handle, out string processName, onFailed))
        {
            return;
        }

        var windowTitle = Resizer.GetWindowTitle(handle);
        var match = GetMatchWindowSize(config.WindowSizes, processName, windowTitle);

        var place = Resizer.GetPlacement(handle);

        // workaround: use GetWindowRect to get actual window coordinates
        place.Rect = place.WindowState == WindowState.Maximized ? place.Rect : Resizer.GetRect(handle);
        UpdateOrSaveConfig(match, processName, windowTitle, place);
    }

    public static bool IsProcessAvailable(IntPtr handle, out string processName, Action<Process, Exception>? onFailed)
    {
        processName = string.Empty;
        if (Resizer.IsChildWindow(handle))
        {
            return false;
        }

        var process = Resizer.GetRealProcess(handle);
        if (process is null)
        {
            return false;
        }

        var success = TryGetProcessName(process, out processName, onFailed);
        if (!success)
        {
            return false;
        }

        return !Resizer.IsInvisibleProcess(processName);
    }

    public static Hotkeys? GetKeys(HotkeysType type) =>
        ConfigFactory.Current.GetKeys(type);

    #region private functions

    private static void MoveMatchWindow(MatchWindowSize match, IntPtr handle)
    {
        if (match.FullMatch != null)
        {
            MoveWindow(handle, match.FullMatch);
            return;
        }

        if (match.PrefixMatch != null)
        {
            MoveWindow(handle, match.PrefixMatch);
            return;
        }

        if (match.SuffixMatch != null)
        {
            MoveWindow(handle, match.SuffixMatch);
            return;
        }

        if (match.WildcardMatch != null)
        {
            MoveWindow(handle, match.WildcardMatch);
        }
    }

    private static bool TryGetProcessName(Process process, out string processName, Action<Process, Exception>? onFailed)
    {
        try
        {
            processName = process.MainModule?.ModuleName ?? string.Empty;
            return true;
        }
        catch (Exception e)
        {
            onFailed?.Invoke(process, e);
            processName = string.Empty;
            return false;
        }
    }

    private static MatchWindowSize GetMatchWindowSize(
        IEnumerable<WindowSize> windowSizes,
        string processName,
        string? title,
        bool onlyAuto = false)
    {
        var windows = windowSizes.Where(w =>
                                     w.Name.Equals(processName, StringComparison.OrdinalIgnoreCase))
                                 .ToList();

        if (onlyAuto)
        {
            windows = windows.Where(w => w.AutoResize).ToList();
        }

        if (string.IsNullOrEmpty(title))
        {
            title = "*";
        }

        return new MatchWindowSize
        {
            FullMatch = windows.FirstOrDefault(w => w.Title == title),
            PrefixMatch = windows.FirstOrDefault(w =>
                w.Title.StartsWith("*") && w.Title.Length > 1 && title!.EndsWith(w.Title.TrimStart('*'))),
            SuffixMatch = windows.FirstOrDefault(w =>
                w.Title.EndsWith("*") && w.Title.Length > 1 && title!.StartsWith(w.Title.TrimEnd('*'))),
            WildcardMatch = windows.FirstOrDefault(w => w.Title.Equals("*"))
        };
    }


    private static void UpdateOrSaveConfig(MatchWindowSize match, string processName, string? title, WindowPlacement placement)
    {
        if (string.IsNullOrWhiteSpace(processName)) return;

        if (match.NoMatch)
        {
            // Add a wildcard match for all titles
            InsertOrder(new WindowSize
            {
                Name = processName,
                Title = "*",
                Rect = placement.Rect,
                State = placement.WindowState,
                MaximizedPosition = placement.MaximizedPosition,
            });
            if (!string.IsNullOrWhiteSpace(title))
            {
                InsertOrder(new WindowSize
                {
                    Name = processName,
                    Title = title!,
                    Rect = placement.Rect,
                    State = placement.WindowState,
                    MaximizedPosition = placement.MaximizedPosition,
                });
            }

            ConfigFactory.Save();
            return;
        }

        if (match.FullMatch != null)
        {
            match.FullMatch.Rect = placement.Rect;
            match.FullMatch.State = placement.WindowState;
            match.FullMatch.MaximizedPosition = placement.MaximizedPosition;
        }
        else if (!string.IsNullOrWhiteSpace(title))
        {
            InsertOrder(new WindowSize
            {
                Name = processName,
                Title = title!,
                Rect = placement.Rect,
                State = placement.WindowState,
                MaximizedPosition = placement.MaximizedPosition,
            });
        }

        if (match.SuffixMatch != null)
        {
            match.SuffixMatch.Rect = placement.Rect;
            match.SuffixMatch.State = placement.WindowState;
            match.SuffixMatch.MaximizedPosition = placement.MaximizedPosition;
        }

        if (match.PrefixMatch != null)
        {
            match.PrefixMatch.Rect = placement.Rect;
            match.PrefixMatch.State = placement.WindowState;
            match.PrefixMatch.MaximizedPosition = placement.MaximizedPosition;
        }

        if (match.WildcardMatch != null)
        {
            match.WildcardMatch.Rect = placement.Rect;
            match.WildcardMatch.State = placement.WindowState;
            match.WildcardMatch.MaximizedPosition = placement.MaximizedPosition;
        }
        else
        {
            InsertOrder(new WindowSize
            {
                Name = processName,
                Title = "*",
                Rect = placement.Rect,
                State = placement.WindowState,
                MaximizedPosition = placement.MaximizedPosition,
            });
        }

        ConfigFactory.Save();
    }

    private static void MoveWindow(IntPtr handle, WindowSize match)
    {
        Resizer.SetPlacement(handle, match.Rect, match.MaximizedPosition, match.State);
    }

/*
        private static void MoveWindow(IntPtr handle, WindowSize match)
        {
            Resizer.MoveWindow(handle, match.Rect);
            if (match.State == WindowState.Maximized)
            {
                Resizer.MaximizeWindow(handle);
            }
        }
*/

    private static void InsertOrder(WindowSize item)
    {
        var list = ConfigFactory.Current.WindowSizes;
        var backing = list.ToList();
        backing.Add(item);
        var index = backing.OrderBy(l => l.Name).ThenBy(l => l.Title).ToList().IndexOf(item);
        list.Insert(index, item);
    }

    #endregion
}
