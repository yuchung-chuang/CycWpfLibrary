using System.Windows;

namespace CycWpfLibrary
{
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

    public static Point ToPoint(this Size size)
    {
      return new Point(size.Width, size.Height);
    }
  }
}
