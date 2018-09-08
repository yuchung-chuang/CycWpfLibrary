using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.NativeMethods
{
  public static class PointExtensions
  {
    public static Point Add(this Point point1, Point point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Minus(this Point point1, Point point2)
    {
      return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }
    
  }
}
