using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CycWpfLibrary
{
  public enum LocType
  {
    /// <summary>
    /// LeftTop
    /// </summary>
    LT,
    /// <summary>
    /// LeftBottom
    /// </summary>
    LB,
    /// <summary>
    /// RightTop
    /// </summary>
    RT,
    /// <summary>
    /// RightBottom
    /// </summary>
    RB,
  }

  public static class Drawing
  {
    public static void DrawRectangle(this Canvas parent, (double X
      , double Y, LocType Type) location, (double Width, double Height) size, Color color)
    {
      switch (location.Type)
      {
        case LocType.LT:
          DrawRectangle(parent, size, new SolidColorBrush(color), location.X, location.Y);
          break;
        case LocType.LB:
          DrawRectangle(parent, size, new SolidColorBrush(color), location.X, double.NaN, double.NaN, location.Y);
          break;
        case LocType.RT:
          DrawRectangle(parent, size, new SolidColorBrush(color), double.NaN, location.Y, location.X);
          break;
        case LocType.RB:
          DrawRectangle(parent, size, new SolidColorBrush(color), double.NaN, double.NaN, location.X, location.Y);
          break;
        default:
          break;
      }
    }
    public static void DrawRectangle(this Canvas parent, Point location, Size size, Color color)
    {
      DrawRectangle(parent, (size.Width, size.Height), new SolidColorBrush(color), location.X, location.Y);
    }
    public static void DrawRectangle(this Canvas parent, Point location, Size size, Brush brush)
    {
      DrawRectangle(parent, (size.Width, size.Height), brush, location.X, location.Y);
    }
    public static void DrawRectangle(this Canvas parent, Rect rect, Brush brush)
    {
      DrawRectangle(parent, (rect.Width, rect.Height), brush, rect.Left, rect.Top);
    }
    internal static void DrawRectangle(Canvas parent, (double Width, double Height) size, Brush brush,
      double left = double.NaN,
      double top = double.NaN,
      double right = double.NaN,
      double bottom = double.NaN)
    {
      var rectangle = new Rectangle()
      {
        Fill = brush,
        Width = size.Width,
        Height = size.Height,
      };
      if (!double.IsNaN(left))
        Canvas.SetLeft(rectangle, left);
      if (!double.IsNaN(right))
        Canvas.SetRight(rectangle, right);
      if (!double.IsNaN(top))
        Canvas.SetTop(rectangle, top);
      if (!double.IsNaN(bottom))
        Canvas.SetBottom(rectangle, bottom);
      parent.Children.Add(rectangle);
    }
  }
}
