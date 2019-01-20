using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Math;

namespace CycWpfLibrary.Media
{
  public static class PointExtensions
  {
    public static double DistanceTo(this Point point1, Point point2)
    {
      return point1.Minus(point2).Norm();
    }

    public static double Norm(this Point point)
    {
      return Sqrt(Pow(point.X, 2) + Pow(point.Y, 2));
    }

    public static Point Add(this Point point1, Point point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Add(this Point point1, (double X, double Y) point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Minus(this Point point1, Point point2)
    {
      return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static Point Minus(this Point point1, (double X, double Y) point2)
    {
      return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }
  }
}
