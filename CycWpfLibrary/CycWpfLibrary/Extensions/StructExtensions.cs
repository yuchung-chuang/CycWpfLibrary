using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Math;

namespace CycWpfLibrary
{
  public static class TupleExtensions
  {
    public static (double, double) Times(this (double, double) tuple, double scale)
    {
      return (tuple.Item1 * scale, tuple.Item2 * scale);
    }

    public static (double, double) Add(this (double, double) tuple1, (double, double) tuple2)
    {
      return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
    }

    public static (double, double) Minus(this (double, double) tuple1, (double, double) tuple2)
    {
      return (tuple1.Item1 - tuple2.Item1, tuple1.Item2 - tuple2.Item2);
    }
  }
  public static class SizeExtensions
  {
    public static Size Add(this Size size1, Size size2)
    {
      return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
    }

    public static Size Add(this Size size1, (double Width, double Height) size2)
    {
      return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
    }

    public static Size Minus(this Size size1, Size size2)
    {
      return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
    }

    public static Size Minus(this Size size1, (double Width, double Height) size2)
    {
      return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
    }

    public static Size Divide(this Size size, double factor)
    {
      return new Size(size.Width / factor, size.Height / factor);
    }
  }
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

    public static Point Times(this Point point, double factor)
    {
      return new Point(point.X * factor, point.Y * factor);
    }

    public static Point Times(this Point poin1, Point point2)
    {
      return new Point(poin1.X * point2.X, poin1.Y * point2.Y);
    }

    public static Point Divide(this Point point, double factor)
    {
      return new Point(point.X / factor, point.Y / factor);
    }

    public static Point Divide(this Point point1, Point point2)
    {
      return new Point(point1.X / point2.X, point1.Y / point2.Y);
    }
  }
}
