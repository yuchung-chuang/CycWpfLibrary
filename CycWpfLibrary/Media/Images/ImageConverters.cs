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


    // Not straight forward
    public static BitmapSource ToBitmapSource(this PixelBitmap pixelBitmap) => pixelBitmap.Bitmap.ToBitmapSource();
    public static PixelBitmap ToPixelBitmap(this BitmapImage bitmapImage) => bitmapImage.ToBitmap().ToPixelBitmap();
    public static PixelBitmap ToPixelBitmap(this BitmapSource bitmapSource) => new PixelBitmap(bitmapSource.ToBitmap());

    #region Helper methods
    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);
    [DllImport("user32.dll")]
    private static extern IntPtr CreateIconIndirect(ref IconInfo piconinfo);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo piconinfo);
    #endregion
  }
}
