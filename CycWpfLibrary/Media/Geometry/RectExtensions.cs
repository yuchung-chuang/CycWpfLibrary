using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.Media
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
  }
}
