using System.Collections.Generic;
using System.Windows;
using static System.Math;

namespace CycWpfLibrary
{
  public class PointInt
  {
    public PointInt(Point point)
    {
      X = Double2Int(point.X);
      Y = Double2Int(point.Y);
    }
    public PointInt(int x, int y)
    {
      X = x;
      Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public static PointInt operator +(Point point1, PointInt point2) 
      => new PointInt(Double2Int(point1.X + point2.X), Double2Int(point1.Y + point2.Y));
    public static PointInt operator +(PointInt point1, Point point2)
      => new PointInt(Double2Int(point1.X + point2.X), Double2Int(point1.Y + point2.Y));
    public static PointInt operator +(PointInt point1, PointInt point2)
      => new PointInt(point1.X + point2.X, point1.Y + point2.Y);
    public static PointInt operator +(PointInt point1, (int X, int Y) point2)
      => new PointInt(point1.X + point2.X, point1.Y + point2.Y);
    public static PointInt operator -(PointInt point1, PointInt point2)
      => new PointInt(point1.X - point2.X, point1.Y - point2.Y);
    public static PointInt operator -(PointInt point1, (int X, int Y) point2)
      => new PointInt(point1.X - point2.X, point1.Y - point2.Y);
    public static bool operator ==(PointInt i1, PointInt i2)
    {
      return EqualityComparer<PointInt>.Default.Equals(i1, i2);
    }
    public static bool operator !=(PointInt i1, PointInt i2)
    {
      return !(i1 == i2);
    }

    public override bool Equals(object obj) 
      => obj is PointInt i &&
             X == i.X &&
             Y == i.Y;
    public override int GetHashCode()
    {
      var hashCode = 1861411795;
      hashCode = hashCode * -1521134295 + X.GetHashCode();
      hashCode = hashCode * -1521134295 + Y.GetHashCode();
      return hashCode;
    }

    public override string ToString() 
      => $"{X}, {Y}";

    private static int Double2Int(double d)
    {
      return (int)Floor(d);
    }
  }
}
