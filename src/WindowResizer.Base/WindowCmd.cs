using System;
using System.Collections.Generic;
using System.Linq;
using WindowResizer.Common.Windows;
using WindowResizer.Configuration;
using WindowResizer.Core.WindowControl;
using static WindowResizer.Base.WindowUtils;

namespace WindowResizer.Base;

public static class WindowCmd
{
    public static void Resize(string? configPath, string? profileName, string? process, string? title, Action<string>? onError)
    {
        var profile = LoadConfig(configPath, profileName, onError);
        if (profile is null)
        {
            onError?.Invoke($"Profile <{profileName}> not exists");
            return;
        }

        var targets = new List<IntPtr>();
        if (string.IsNullOrWhiteSpace(process))
        {
            var fp = Resizer.GetForegroundHandle();
            targets.Add(fp);
        }
        else
        {
            var processes = Resizer.GetOpenWindows();

            foreach (var handler in processes)
            {
                if (!IsProcessAvailable(handler, out string name, null))
                {
                    continue;
                }

                if (!name.Equals(process, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var t = Resizer.GetWindowTitle(handler);
                if (string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(title))
                {
                    targets.Add(handler);
                }
                else if (!string.IsNullOrWhiteSpace(title) && t!.Contains(title))
                {
                    targets.Add(handler);
                }
            }
        }

        foreach (var tp in targets)
        {
            ResizeWindow(tp, profile, (p, e) =>
            {
                onError?.Invoke($"Unable to resize process <{p}>, elevated privileges may be required.");
            }, (p, t) =>
            {
                onError?.Invoke($"No saved settings for <{p} :: {t}>.");
            });
        }
    }

    public static void ResizeAll(string? configPath, string? profileName, Action<string>? onError)
    {
        var profile = LoadConfig(configPath, profileName, onError);
        if (profile is null)
        {
            onError?.Invoke($"Profile <{profileName}> not exists");
            return;
        }

        var windows = Resizer.GetOpenWindows();
        windows.Reverse();
        foreach (var window in windows)
        {
            if (Resizer.GetWindowState(window) != WindowState.Minimized)
            {
                ResizeWindow(window, profile, null, null);
            }
        }
    }

    private static Config? LoadConfig(string? configPath, string? profileName, Action<string>? onError)
    {
        if (!ConfigUtils.Load(configPath, onError))
        {
            return null;
        }

        if (string.IsNullOrEmpty(profileName))
        {
            return ConfigFactory.Current;
        }

        return ConfigFactory.Profiles.Configs.FirstOrDefault(i =>
            i.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase));
    }
}
