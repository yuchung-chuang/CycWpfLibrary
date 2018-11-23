using Microsoft.Win32.SafeHandles;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;
using PointWinForm = System.Drawing.Point;
using PixelFormatsWpf = System.Windows.Media.PixelFormats;
using PixelFormatWinForm = System.Drawing.Imaging.PixelFormat;
using System.Drawing.Imaging;

namespace CycWpfLibrary.Media
{
  #region Helper classes
  internal struct IconInfo
  {
    public bool fIcon;
    public int xHotspot;
    public int yHotspot;
    public IntPtr hbmMask;
    public IntPtr hbmColor;
  }
  internal class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DestroyIcon([In] IntPtr hIcon);
    private SafeIconHandle()
        : base(true)
    {
    }
    public SafeIconHandle(IntPtr hIcon)
        : base(true)
    {
      this.SetHandle(hIcon);
    }
    protected override bool ReleaseHandle()
    {
      return DestroyIcon(this.handle);
    }
  }
  #endregion

  public static class ImageConverters
  {
    // BitmapImage
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
    public static PixelBitmap ToPixelBitmap(this BitmapImage bitmapImage) => bitmapImage.ToBitmap().ToPixelBitmap();

    // Bitmap
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
    public static PixelBitmap ToPixelBitmap(this Bitmap bitmap) => new PixelBitmap(bitmap);
    public static Cursor ToCursor(this Bitmap b, int x, int y)
    {
      /// get icon from input bitmap
      IconInfo ico = new IconInfo();
      GetIconInfo(b.GetHicon(), ref ico);

      /// set the hotspot
      ico.xHotspot = x;
      ico.yHotspot = y;
      ico.fIcon = false;

      /// create a cursor from iconinfo
      IntPtr cursor = CreateIconIndirect(ref ico);
      return CursorInteropHelper.Create(new SafeIconHandle(cursor));
    }

    // PixelBitamp
    public static BitmapSource ToBitmapSource(this PixelBitmap pixelBitmap) => pixelBitmap.Bitmap.ToBitmapSource();

    // BitmapSource
    public static Bitmap ToBitmap(this BitmapSource bitmapsource)
    {
      //convert image format
      var src = new FormatConvertedBitmap();
      src.BeginInit();
      src.Source = bitmapsource;
      src.DestinationFormat = PixelFormatsWpf.Bgra32;
      src.EndInit();

      //copy to bitmap
      Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, PixelFormatWinForm.Format32bppArgb);
      var data = bitmap.LockBits(new Rectangle(PointWinForm.Empty, bitmap.Size), ImageLockMode.WriteOnly, PixelFormatWinForm.Format32bppArgb);
      src.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
      bitmap.UnlockBits(data);

      return bitmap;
    }


    #region Helper methods
    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);
    [DllImport("user32.dll")]
    private static extern IntPtr CreateIconIndirect(ref IconInfo piconinfo);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo piconinfo);
    #endregion

    #region Deprecated methods
    [Obsolete("Need to be fixed.", true)]
    public static PixelBitmap ToPixelBitmap(this BitmapSource bitmapSource)
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
    #endregion
  }
}
