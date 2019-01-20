﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.Media
{
  public static class BitmapExtensions
  {
    public static Bitmap Crop(this Bitmap bitmap, Rectangle rect)
    {
      var output = new Bitmap(rect.Width, rect.Height);
      using (Graphics g = Graphics.FromImage(output))
        g.DrawImage(bitmap, -rect.X, -rect.Y);
      return output;
    }

    public static Bitmap Crop(this Bitmap bitmap, Rect rect)
    {
      return bitmap.Crop(new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
    }

    public static Bitmap SetPixelFormat(this Bitmap bitmap, PixelFormat pixelFormat)
    {
      var bitmapFormatted = new Bitmap(bitmap.Width, bitmap.Height, pixelFormat);
      using (Graphics g = Graphics.FromImage(bitmapFormatted))
      {
        g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
      }
      return bitmapFormatted;
    }
  }
}