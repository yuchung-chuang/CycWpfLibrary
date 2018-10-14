using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Colors = System.Windows.Media.Colors;
using Grid = System.Windows.Controls.Grid;
using Image = System.Windows.Controls.Image;
using Window = System.Windows.Window;

namespace CycWpfLibrary.Media
{
  public class PixelBitmap : ICloneable
  {
    public int Width => _Bitmap.Width;
    public int Height => _Bitmap.Height;
    public Size Size => _Bitmap.Size;
    public static PixelFormat PixelImageFormat = PixelFormat.Format32bppArgb;
    public readonly int Byte = 4; //根據Format32bppArgb
    public int Stride => Width * Byte;  //根據Format32bppArgb

    private Bitmap _Bitmap;
    public Bitmap Bitmap
    {
      get => _Bitmap;
      set
      {
        _Bitmap = value;
        BitmapToPixel();
      }
    }
    private byte[] _Pixel;
    public byte[] Pixel
    {
      get => _Pixel;
      set
      {
        _Pixel = value;
        PixelToBitmap();
        PixelToPixel3();
      }
    }
    private byte[,,] _Pixel3;
    public byte[,,] Pixel3
    {
      get => _Pixel3;
      set
      {
        _Pixel3 = value;
        Pixel3ToPixel();
      }
    }

    #region Constructors
    public PixelBitmap()
    {

    }
    public PixelBitmap(Bitmap bitmap)
    {
      //轉換PixelFormat
      if (bitmap.PixelFormat != PixelImageFormat)
      {
        var bitmapFormatted = new Bitmap(bitmap.Width, bitmap.Height, PixelImageFormat);
        using (Graphics g = Graphics.FromImage(bitmapFormatted))
        {
          g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }
        bitmap = bitmapFormatted;
      }

      _Bitmap = new Bitmap(bitmap);
      Synchronize(nameof(Bitmap)); //更新pixel
    }
    public PixelBitmap(Size size)
    {
      _Bitmap = new Bitmap(size.Width, size.Height, PixelImageFormat); //不更新pixel
    }
    public PixelBitmap(byte[] pixel, Size size)
    {
      _Bitmap = new Bitmap(size.Width, size.Height, PixelImageFormat); //不更新pixel
      _Pixel = pixel;
      Synchronize(nameof(Pixel)); //更新bitmap
    }
    public PixelBitmap(byte[,,] pixel3)
    {
      var width = pixel3.GetLength(0);
      var height = pixel3.GetLength(1);
      _Pixel3 = pixel3;
      _Bitmap = new Bitmap(width, height, PixelImageFormat); //不更新pixel
      Synchronize(nameof(Pixel3));
    }
    public object Clone()
    {
      if (this._Bitmap == null)
      {
        return new PixelBitmap();
      }
      else
      {
        return new PixelBitmap()
        {
          _Bitmap = (Bitmap)this._Bitmap.Clone(),
          _Pixel = (byte[])this._Pixel.Clone(),
          _Pixel3 = (byte[,,])this._Pixel3.Clone(),
        };

      }
    } //比new PixelIamge(bitmap)快
    #endregion

    #region Public Methods
    public void Synchronize(string property = null)
    {
      switch (property)
      {
        default:
        case nameof(Pixel):
          PixelToBitmap();
          PixelToPixel3();
          break;
        case nameof(Bitmap):
          BitmapToPixel();
          PixelToPixel3();
          break;
        case nameof(Pixel3):
          Pixel3ToPixel();
          PixelToBitmap();
          break;
      }
    }

    public void ShowSnapShot()
    {
      var window = new Window
      {
        Background = new SolidColorBrush(Colors.LightGray),
      };
      var grid = new Grid
      {
        Background = null,
      };
      var image = new Image();
      window.Content = grid;
      grid.Children.Add(image);
      image.Source = this.ToBitmapSource();
      window.ShowDialog();
    }
    #endregion

    #region Convert Methods
    private void PixelToPixel3()
    {
      _Pixel3 = new byte[_Bitmap.Width, _Bitmap.Height, Byte];
      int idx;
      for (int x = 0; x < _Bitmap.Width; x++)
      {
        for (int y = 0; y < _Bitmap.Height; y++)
        {
          idx = x * Byte + y * Stride;
          _Pixel3[x, y, 3] = _Pixel[idx]; //B
          _Pixel3[x, y, 2] = _Pixel[idx+1]; //G
          _Pixel3[x, y, 1] = _Pixel[idx+2]; //R
          _Pixel3[x, y, 0] = _Pixel[idx+3]; //A
        }
      }
    }

    private void Pixel3ToPixel()
    {
      var width = _Pixel3.GetLength(0);
      var height = _Pixel3.GetLength(1);
      var depth = _Pixel3.GetLength(2);
      _Pixel = new byte[width * height * depth];
      int idx;
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          idx = x * Byte + y * Stride;
          _Pixel[idx]     = _Pixel3[x, y, 3]; //B
          _Pixel[idx + 1] = _Pixel3[x, y, 2]; //G
          _Pixel[idx + 2] = _Pixel3[x, y, 1]; //R
          _Pixel[idx + 3] = _Pixel3[x, y, 0]; //A
        }
      }
    }

    private void PixelToBitmap()
    {
      //將image鎖定到系統內的記憶體的某個區塊中，並將這個結果交給BitmapData類別的imageData
      BitmapData bitmapData = _Bitmap.LockBits(
      new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height),
      ImageLockMode.ReadOnly,
      PixelImageFormat);

      //複製pixel到bitmapData中
      Marshal.Copy(_Pixel, 0, bitmapData.Scan0, _Pixel.Length);

      //解鎖
      _Bitmap.UnlockBits(bitmapData);
    }

    private void BitmapToPixel()
    {
      //將image鎖定到系統內的記憶體的某個區塊中，並將這個結果交給BitmapData類別的imageData
      BitmapData bitmapData = _Bitmap.LockBits(
        new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height),
        ImageLockMode.ReadOnly,
        PixelImageFormat);

      //初始化pixel陣列，用來儲存所有像素的訊息
      _Pixel = new byte[bitmapData.Stride * _Bitmap.Height];

      //複製imageData的RGB信息到pixel陣列中
      Marshal.Copy(bitmapData.Scan0, _Pixel, 0, _Pixel.Length);

      //解鎖
      _Bitmap.UnlockBits(bitmapData); //其他地方正在使用物件.....

    }
    #endregion
  }
}
