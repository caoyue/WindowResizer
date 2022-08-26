namespace WindowResizer.Common.Windows;

public struct Rect
{
    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public override string ToString()
    {
        return $"{this.Left},{this.Top},{this.Right},{this.Bottom}";
    }
}
