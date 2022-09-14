namespace WindowResizer.Common.Windows;

public struct WindowPlacement
{
    public Rect Rect { get; set; }

    public WindowState WindowState { get; set; }

    public Point MaximizedPosition { get; set; }
}
