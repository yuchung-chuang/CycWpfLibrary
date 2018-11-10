using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.WinForm
{
  public static class RectangleExtenstions
  {
    public static Rectangle ScaleRectangle(this Rectangle r, double scale)
    {
      double centerX = r.Location.X + r.Width / 2.0;
      double centerY = r.Location.Y + r.Height / 2.0;
      double newWidth = System.Math.Round(r.Width * scale);
      double newHeight = System.Math.Round(r.Height * scale);
      return new Rectangle((int)System.Math.Round(centerX - newWidth / 2.0), (int)System.Math.Round(centerY - newHeight / 2.0),
         (int)newWidth, (int)newHeight);
    }
  }
}
