using System;
using Newtonsoft.Json;
using WindowResizer.Common.Windows;

namespace WindowResizer.Configuration;

public class WindowSize : IComparable<WindowSize>
{
    public string Name { get; set; } = String.Empty;

    public string Title { get; set; } = String.Empty;

    public Rect Rect { get; set; }

    public WindowState State { get; set; } = WindowState.Normal;

    public Point MaximizedPosition { get; set; } = new(0, 0);

    public bool AutoResize { get; set; }

    // AutoResize Delay Milliseconds
    public int AutoResizeDelay { get; set; } = 0;

    public int CompareTo(WindowSize? other)
    {
        var c = string.Compare(other?.Name ?? String.Empty, Name, StringComparison.Ordinal);
        return c == 0 ? string.Compare(other?.Title ?? String.Empty, Title, StringComparison.Ordinal) : c;
    }

    #region properties

    [JsonIgnore]
    public int Top
    {
        get { return Rect.Top; }
        set
        {
            Rect = Rect with
            {
                Top = value
            };
        }
    }

    [JsonIgnore]
    public int Left
    {
        get { return Rect.Left; }
        set
        {
            Rect = Rect with
            {
                Left = value
            };
        }
    }

    [JsonIgnore]
    public int Right
    {
        get { return Rect.Right; }
        set
        {
            Rect = Rect with
            {
                Right = value
            };
        }
    }

    [JsonIgnore]
    public int Bottom
    {
        get { return Rect.Bottom; }
        set
        {
            Rect = Rect with
            {
                Bottom = value
            };
        }
    }

    #endregion
}
