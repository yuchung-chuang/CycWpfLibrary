using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Colors = System.Windows.Media.Colors;
using DependencyObject = System.Windows.DependencyObject;
using Grid = System.Windows.Controls.Grid;
using Image = System.Windows.Controls.Image;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Window = System.Windows.Window;

namespace CycWpfLibrary.Media
{
  public class PixelBitmap : DependencyObject, ICloneable
  {
    public int Width;
    public int Height;
    public Size Size;
    public readonly int Byte = 4; //根據Format32bppArgb
    public int Stride;  //根據Format32bppArgb
    public static PixelFormat PixelImageFormat = PixelFormat.Format32bppArgb;

    private static readonly object key = new object();
    private Bitmap bitmap;
    /// <summary>
    /// 自動設定<see cref="Width"/>, <see cref="Height"/>, <see cref="Size"/>, <see cref="Stride"/>等欄位。
    /// </summary>
    private Bitmap _Bitmap
    {
      get => bitmap;
      set
      {
        bitmap = value;
        Width = _Bitmap.Width;
        Height = _Bitmap.Height;
        Size = _Bitmap.Size;
        Stride = _Bitmap.Width * Byte;
      }
    }
    /// <summary>
    /// <seealso cref="System.Drawing.Bitmap"/>物件不可同時被多執行緒使用，需要使用互斥鎖<seealso cref="lock"/>，且getter需回傳副本。
    /// </summary>
    public Bitmap Bitmap
    {
      get
      {
        lock (key) 
        {
          return _Bitmap.Clone() as Bitmap;

        }
      }
      set
      {
        lock (key)
        {
          _Bitmap = value;
          Sync(nameof(Bitmap));
        }
      }
    }
    private byte[] _Pixel;
    public byte[] Pixel
    {
      get
      {
          return _Pixel;
      }
      set
      {
          _Pixel = value;
          Sync(nameof(Pixel));
      }
    }
    private byte[,,] _Pixel3;
    public byte[,,] Pixel3
    {
      get
      {
          return _Pixel3;
      }

      set
      {
          _Pixel3 = value;
          Sync(nameof(Pixel3));
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
      Sync(nameof(Bitmap)); //更新pixel
    }
    public PixelBitmap(Size size)
    {
      _Bitmap = new Bitmap(size.Width, size.Height, PixelImageFormat); //不更新pixel
    }
    public PixelBitmap(byte[] pixel, Size size)
    {
      _Bitmap = new Bitmap(size.Width, size.Height, PixelImageFormat); //不更新pixel
      _Pixel = pixel;
      Sync(nameof(Pixel)); //更新bitmap
    }
    public PixelBitmap(byte[,,] pixel3)
    {
      var width = pixel3.GetLength(0);
      var height = pixel3.GetLength(1);
      _Pixel3 = pixel3;
      _Bitmap = new Bitmap(width, height, PixelImageFormat); //不更新pixel
      Sync(nameof(Pixel3));
    }
    public object Clone()
    {
      if (_Bitmap == null)
      {
        return new PixelBitmap();
      }
      else
      {
        var pixelbitmap = new PixelBitmap();
        pixelbitmap._Bitmap = Bitmap;
        pixelbitmap._Pixel = _Pixel.Clone() as byte[];
        pixelbitmap._Pixel3 = _Pixel3.Clone() as byte[,,];
        return pixelbitmap;
      }
    } //比new PixelIamge(bitmap)快
    #endregion

    #region Public Methods
    public void Sync(string property = null)
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
          PixelToBitmap();
          Pixel3ToPixel();
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
      var pixel = _Pixel.Clone() as byte[];
      var width = Width;
      var height = Height;
      var pixel3 = new byte[width, height, Byte];
      int idx;
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          idx = x * Byte + y * Stride;
          pixel3[x, y, 3] = pixel[idx]; //B
          pixel3[x, y, 2] = pixel[idx + 1]; //G
          pixel3[x, y, 1] = pixel[idx + 2]; //R
          pixel3[x, y, 0] = pixel[idx + 3]; //A
        }
      }
      _Pixel3 = pixel3;
    }

    private void Pixel3ToPixel()
    {
      var pixel3 = _Pixel3.Clone() as byte[,,];
      var width = pixel3.GetLength(0);
      var height = pixel3.GetLength(1);
      var depth = pixel3.GetLength(2);
      var pixel = new byte[width * height * depth];
      int idx;
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          idx = x * Byte + y * Stride;
          pixel[idx] = pixel3[x, y, 3]; //B
          pixel[idx + 1] = pixel3[x, y, 2]; //G
          pixel[idx + 2] = pixel3[x, y, 1]; //R
          pixel[idx + 3] = pixel3[x, y, 0]; //A
        }
      }
      _Pixel = pixel;
    }

    private void PixelToBitmap()
    {
      //將image鎖定到系統內的記憶體的某個區塊中，並將這個結果交給BitmapData類別的imageData
      BitmapData bitmapData = bitmap.LockBits(
      new Rectangle(0, 0, Width, Height),
      ImageLockMode.ReadOnly,
      PixelImageFormat);

      //複製pixel到bitmapData中
      Marshal.Copy(_Pixel, 0, bitmapData.Scan0, _Pixel.Length);

      //解鎖
      bitmap.UnlockBits(bitmapData);
    }

    private void BitmapToPixel()
    {
      //將image鎖定到系統內的記憶體的某個區塊中，並將這個結果交給BitmapData類別的imageData
      BitmapData bitmapData = bitmap.LockBits(
        new Rectangle(0, 0, Width, Height),
        ImageLockMode.ReadOnly,
        PixelImageFormat);

      //初始化pixel陣列，用來儲存所有像素的訊息
      _Pixel = new byte[bitmapData.Stride * Height];

      //複製imageData的RGB信息到pixel陣列中
      Marshal.Copy(bitmapData.Scan0, _Pixel, 0, _Pixel.Length);

      //解鎖
      bitmap.UnlockBits(bitmapData); //其他地方正在使用物件.....

    }
    #endregion
  }
}
