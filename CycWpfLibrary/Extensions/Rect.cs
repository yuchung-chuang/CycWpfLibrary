using System.Windows;

namespace CycWpfLibrary
{
  public static class RectExtensions
  {
    public static Rect Minus(this Rect rect, Point point)
    {
      return new Rect(rect.Location.Minus(point), rect.Size);
    }

    public static Rect Minus(this Rect rect, Vector vector)
    {
      return new Rect(rect.Location - vector, rect.Size);
    }

    public static Rect Minus(this Rect rect, (double X, double Y) point)
    {
      return new Rect(rect.Location.Minus(point), rect.Size);
    }

    public static Rect Divide(this Rect rect, double factor)
    {
      return new Rect(rect.Location.Divide(factor), rect.Size.Divide(factor));
    }

    public static Rect Divide(this Rect rect, Point point)
    {
      return new Rect(rect.X / point.X, rect.Y / point.Y, rect.Width / point.X, rect.Height / point.Y);
    }

  }
}
