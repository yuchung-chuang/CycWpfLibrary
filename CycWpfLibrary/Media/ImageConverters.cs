using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

namespace CycWpfLibrary.Media
{
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
    public static PixelBitmap ToPixelBitmap(this Bitmap bitmap) => new PixelBitmap(bitmap);

    public struct IconInfo
    {
      public bool fIcon;
      public int xHotspot;
      public int yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }
    [DllImport("user32.dll")]
    static extern IntPtr CreateIconIndirect(ref IconInfo piconinfo);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo piconinfo);
    class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
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

    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this Bitmap bitmap) 
      where TColor : struct, IColor 
      where TDepth : new() 
      => new Image<TColor, TDepth>(bitmap);
    /// <summary>
    /// 將<see cref="Bitmap"/>透過<see cref="Image"/>(<see cref="Bgr"/>,<see cref="byte"/>)轉換成<see cref="Mat"/>
    /// </summary>
    public static Mat ToMat(this Bitmap bitmap) => bitmap.ToImage<Bgr, byte>().Mat;


    // PixelBitamp
    public static BitmapSource ToBitmapSource(this PixelBitmap pixelBitmap)
    {
      return pixelBitmap.Bitmap.ToBitmapSource();
    }

    public static Mat ToMat(this PixelBitmap pixelBitmap) => pixelBitmap.Bitmap.ToMat();

    // Mat
    public static BitmapSource ToBitmapSource(this Mat mat) => mat?.Bitmap?.ToBitmapSource();

    public static Bitmap ToBitmap(this Mat mat) => mat?.Bitmap;

    public static PixelBitmap ToPixelBitmap(this Mat mat) => mat?.Bitmap?.ToPixelBitmap();

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
