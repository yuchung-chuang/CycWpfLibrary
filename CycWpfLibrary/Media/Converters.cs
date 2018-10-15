using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

namespace CycWpfLibrary.Media
{
  public static class Converters
  {
    public static Bitmap ToBitmap(this BitmapImage bitmapImage)
    {
      using (MemoryStream outStream = new MemoryStream())
      {
        var enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(bitmapImage));
        enc.Save(outStream);
        return new Bitmap(outStream);
      }
    }
    public static PixelBitmap ToPixelBitmap(this BitmapImage bitmapImage)
    {
      return new PixelBitmap(bitmapImage.ToBitmap());
    }

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);
    public static BitmapSource ToBitmapSource(this Bitmap bitmap)
    {
      IntPtr hBitmap = bitmap.GetHbitmap();
      BitmapSource retval;

      try
      {
        retval = Imaging.CreateBitmapSourceFromHBitmap(
                     hBitmap,
                     IntPtr.Zero,
                     Int32Rect.Empty,
                     BitmapSizeOptions.FromEmptyOptions());
      }
      finally
      {
        DeleteObject(hBitmap);
      }

      return retval;
    }
    
    public static PixelBitmap ToPixelBitmap(this Bitmap bitmap)
    {
      return new PixelBitmap(bitmap);
    }

    public static BitmapSource ToBitmapSource(this PixelBitmap pixelBitmap)
    {
      return pixelBitmap.Bitmap.ToBitmapSource();
    }

    /// <summary>
    /// ???
    /// </summary>
    [Obsolete("Need to be fixed.", true)]
    private static PixelBitmap ToPixelBitmap(this BitmapSource bitmapSource)
    {
      var encoder = new BmpBitmapEncoder();
      var memoryStream = new MemoryStream();
      encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
      encoder.Save(memoryStream);
      var pixel = memoryStream.GetBuffer();
      return new PixelBitmap(pixel, new Size((int)bitmapSource.Width, (int)bitmapSource.Height));
    }
    [Obsolete("Need to be fixed.", true)]
    public static BitmapImage ToBitmapImage(this BitmapSource bitmapSource)
    {
      BmpBitmapEncoder encoder = new BmpBitmapEncoder();
      MemoryStream memoryStream = new MemoryStream();
      BitmapImage bImg = new BitmapImage();

      encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
      encoder.Save(memoryStream);

      memoryStream.Position = 0;
      bImg.BeginInit();
      bImg.StreamSource = memoryStream;
      bImg.EndInit();

      memoryStream.Close();

      return bImg;
    }
  }
}
