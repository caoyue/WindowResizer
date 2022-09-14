using System;
using System.Collections.Generic;
using System.Linq;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Common.Windows;
using WindowResizer.Configuration;
using WindowResizer.Core.WindowControl;

namespace WindowResizer.Utils
{
    internal class WindowUtils
    {
        internal static MatchWindowSize GetMatchWindowSize(
            IEnumerable<WindowSize> windowSizes,
            string processName,
            string title,
            bool onlyAuto = false)
        {
            var windows = windowSizes.Where(w =>
                                         w.Name.Equals(processName, StringComparison.OrdinalIgnoreCase))
                                     .ToList();

            if (onlyAuto)
            {
                windows = windows.Where(w => w.AutoResize).ToList();
            }

            return new MatchWindowSize
            {
                FullMatch = windows.FirstOrDefault(w => w.Title == title),
                PrefixMatch = windows.FirstOrDefault(w =>
                    w.Title.StartsWith("*") && w.Title.Length > 1 && title.EndsWith(w.Title.TrimStart('*'))),
                SuffixMatch = windows.FirstOrDefault(w =>
                    w.Title.EndsWith("*") && w.Title.Length > 1 && title.StartsWith(w.Title.TrimEnd('*'))),
                WildcardMatch = windows.FirstOrDefault(w => w.Title.Equals("*"))
            };
        }

        internal static void MoveMatchWindow(MatchWindowSize match, IntPtr handle)
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

        internal static void UpdateOrSaveConfig(MatchWindowSize match, string processName, string title, WindowPlacement placement)
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
                        Title = title,
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
                    Title = title,
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

        internal static Hotkeys GetKeys(HotkeysType type) =>
            ConfigFactory.Current.GetKeys(type);

        private static void MoveWindow(IntPtr handle, WindowSize match)
        {
            Resizer.SetPlacement(handle, match.Rect, match.MaximizedPosition, match.State);
        }

        private static void InsertOrder(WindowSize item)
        {
            var list = ConfigFactory.Current.WindowSizes;
            var backing = list.ToList();
            backing.Add(item);
            var index = backing.OrderBy(l => l.Name).ThenBy(l => l.Title).ToList().IndexOf(item);
            list.Insert(index, item);
        }
    }
}
