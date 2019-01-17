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
    public static void EraseImage(this Image<Bgra, byte> image, Rect rect)
    {
      CvInvoke.Rectangle(image, rect.ToWinForm(), Bgras.Transparent.MCvScalar, -1);
    }
  }
}
