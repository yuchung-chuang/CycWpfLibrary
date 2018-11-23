using Emgu.CV;
using Emgu.CV.Structure;
using Bitmap = System.Drawing.Bitmap;
using PixelFormatWinForm = System.Drawing.Imaging.PixelFormat;
using System.Windows.Media.Imaging;
using CycWpfLibrary.Media;
using System.Runtime.InteropServices;

namespace CycWpfLibrary.Emgu
{
  public static class ImageConverters
  {
    // Bitmap
    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this Bitmap bitmap)
      where TColor : struct, IColor
      where TDepth : new()
      => new Image<TColor, TDepth>(bitmap);
    public static Mat ToMat(this Bitmap bitmap) => bitmap.ToImage<Bgr, byte>().Mat;

    // PixelBitmap
    public static Mat ToMat(this PixelBitmap pixelBitmap) => pixelBitmap.Bitmap.ToMat();
    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this PixelBitmap pixelBitmap)
      where TColor : struct, IColor
      where TDepth : new()
      => pixelBitmap.Bitmap.ToImage<TColor, TDepth>();

    // Mat
    public static BitmapSource ToBitmapSource(this Mat mat) => mat?.Bitmap?.ToBitmapSource();
    public static Bitmap ToBitmap(this Mat mat)
    {
      return mat?.Bitmap;
    }
    public static PixelBitmap ToPixelBitmap(this Mat mat) => mat?.Bitmap?.ToPixelBitmap();

    // Image(Emgu)
    public static PixelBitmap ToPixelBitmap<TColor, TDepth>(this Image<TColor, TDepth> image)
      where TColor : struct, IColor
      where TDepth : new()
    => new PixelBitmap(image.Data as byte[,,]);

    public static BitmapSource ToBitmapSource(this Image<Bgra, byte> image) => image.Bytes.ToBitmap(image.Width, image.Height).ToBitmapSource();

    // Byte Array(imgData)
    public static Bitmap ToBitmap(this byte[] imgData, int width, int height)
    {
      return new Bitmap(width, height, width * 4,
         PixelFormatWinForm.Format32bppArgb,
         Marshal.UnsafeAddrOfPinnedArrayElement(imgData, 0));
    }
  }
}
