namespace WindowResizer.Common.Windows;

public struct Rect
{
    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public override string ToString()
    {
        return $"{this.Top},{this.Left},{this.Right},{this.Bottom}";
    }
}
