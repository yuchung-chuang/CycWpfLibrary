using static System.Math;
using System.Windows;

namespace CycWpfLibrary
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

    public static Point Add(this Point point, double num)
    {
      return new Point(point.X + num, point.Y + num);
    }

    public static Point Add(this Point point1, Point point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Add(this Point point1, (double X, double Y) point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Minus(this Point point, double num)
    {
      return new Point(point.X - num, point.Y - num);
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

    public static Size ToSize(this Point point)
    {
      return new Size(point.X, point.Y);
    }
  }
}
