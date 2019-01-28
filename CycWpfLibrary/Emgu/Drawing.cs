using CycWpfLibrary.WinForm;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.Emgu
{
  public static class Drawing
  {
    public static void EraseImageSelf(this Image<Bgra, byte> image, Rect rect)
    {
      CvInvoke.Rectangle(image, rect.ToWinForm(), Bgras.Transparent.MCvScalar, -1);
    }

    public async static Task EraseImageSelfAsync(this Image<Bgra, byte> image, Rect rect)
    {
      await Task.Run(() => CvInvoke.Rectangle(image, rect.ToWinForm(), Bgras.Transparent.MCvScalar, -1));
    }

    public static Image<Bgra, byte> EraseImage(this Image<Bgra, byte> image, Rect rect)
    {
      var optImage = image.CopyBlank();
      CvInvoke.Rectangle(optImage, rect.ToWinForm(), Bgras.Transparent.MCvScalar, -1);
      return optImage;
    }

    public async static Task<Image<Bgra, byte>> EraseImageAsync(this Image<Bgra, byte> image, Rect rect)
    {
      var optImage = image.CopyBlank();
      await Task.Run(() => optImage = image.EraseImage(rect));
      return optImage;
    }
  }
}
