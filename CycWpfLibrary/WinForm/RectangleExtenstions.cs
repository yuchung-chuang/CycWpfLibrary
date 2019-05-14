using System.Drawing;

namespace CycWpfLibrary
{
  public static class RectangleExtenstions
  {
    public static Rectangle ScaleRectangle(this Rectangle r, double scale)
    {
      var centerX = r.Location.X + r.Width / 2.0;
      var centerY = r.Location.Y + r.Height / 2.0;
      var newWidth = System.Math.Round(r.Width * scale);
      var newHeight = System.Math.Round(r.Height * scale);
      return new Rectangle((int)System.Math.Round(centerX - newWidth / 2.0), (int)System.Math.Round(centerY - newHeight / 2.0),
         (int)newWidth, (int)newHeight);
    }
  }
}
