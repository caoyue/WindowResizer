using System.Runtime.InteropServices;

namespace WindowResizer.Common.Windows;

[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public int X { get; set; }

    public int Y { get; set; }
}
