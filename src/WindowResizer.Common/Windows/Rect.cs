namespace WindowResizer.Common.Windows;

public struct Rect
{
    public Rect(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public override string ToString()
    {
        return $"{this.Top},{this.Left},{this.Right},{this.Bottom}";
    }
}
