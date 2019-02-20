using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bitmap = System.Drawing.Bitmap;
using ColorWpf = System.Windows.Media.Color;
using PixelFormatWinForm = System.Drawing.Imaging.PixelFormat;

namespace CycWpfLibrary.Emgu
{
  public static class ImageConverters
  {
    // Bitmap
    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this Bitmap bitmap)
      where TColor : struct, IColor
      where TDepth : new()
      => new Image<TColor, TDepth>(bitmap);

    // Image(Emgu)
    public static PixelBitmap ToPixelBitmap<TColor, TDepth>(this Image<TColor, TDepth> image)
      where TColor : struct, IColor
      where TDepth : new()
    => (image.Data as byte[,,]).ToPixelBitmap();

    // Mat
    public static Bitmap ToBitmap(this Mat mat) => mat?.Bitmap;

    // Byte Array(imgData)
    public static Bitmap ToBitmap(this byte[] imgData, int width, int height)
    {
      return new Bitmap(width, height, width * 4,
         PixelFormatWinForm.Format32bppArgb,
         Marshal.UnsafeAddrOfPinnedArrayElement(imgData, 0));
    }

    /// <summary>
    /// 將Emgu.Image.Data設定至PixelBitmap.Pixel
    /// </summary>
    /// <param name="bgraData"><see cref="Emgu"/>.<see cref="Image"/>.Data</param>
    /// <remarks>以三維陣列設定圖像像素陣列，像素深度按B,G,R,A排列</remarks>
    public static PixelBitmap ToPixelBitmap(this byte[,,] bgraData)
    {
      var pixel3 = bgraData.Clone() as byte[,,];
      var height = pixel3.GetLength(0);
      var width = pixel3.GetLength(1);
      var depth = pixel3.GetLength(2); //iptImage depth
      var stride = depth * width; //iptImage stride
      var pixelBitmap = new PixelBitmap(new Size(width, height)); //不更新pixel
      var pixel = new byte[width * height * pixelBitmap.Depth];
      int idx;
      for (int row = 0; row < height; row++)
        for (int col = 0; col < width; col++)
        {
          idx = col * pixelBitmap.Depth + row * pixelBitmap.Stride;
          pixel[idx + 0] = pixel3[row, col, 0]; //B
          pixel[idx + 1] = pixel3[row, col, 1]; //G
          pixel[idx + 2] = pixel3[row, col, 2]; //R
          pixel[idx + 3] = pixel3[row, col, 3]; //A
        }
      pixelBitmap.Pixel = pixel; // Update Bitmap
      return pixelBitmap;
    }

    // Not straight forward
    public static BitmapSource ToBitmapSource(this Mat mat) => mat?.Bitmap?.ToBitmapSource();
    public static PixelBitmap ToPixelBitmap(this Mat mat) => mat?.Bitmap?.ToPixelBitmap();
    public static Mat ToMat(this PixelBitmap pixelBitmap) => pixelBitmap.Bitmap.ToMat();
    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this PixelBitmap pixelBitmap)
      where TColor : struct, IColor
      where TDepth : new()
    {
      return pixelBitmap.Bitmap.ToImage<TColor, TDepth>();
    }
    public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this BitmapSource bitmapSource) 
      where TColor : struct, IColor
      where TDepth : new()
    {
      return bitmapSource.ToBitmap().ToImage<TColor, TDepth>();
    }
    public static Mat ToMat(this Bitmap bitmap) => bitmap.ToImage<Bgr, byte>().Mat;
    public static BitmapSource ToBitmapSource(this Image<Bgra, byte> image) => image.Bytes.ToBitmap(image.Width, image.Height).ToBitmapSource();

    private static readonly BitmapSource sourceSample = BitmapSource.Create(2, 2, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new List<ColorWpf> { Colors.Transparent }), new byte[] { 0, 0, 0, 0 }, 1);
    public static async Task<BitmapSource> ToBitmapSourceAsync(this Image<Bgra, byte> image)
    {
      var source = sourceSample;
      await Task.Run(() =>
      {
        source = image.Bytes.ToBitmap(image.Width, image.Height).ToBitmapSource();
      });
      return source;
    }
  }
}
