using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace CycWpfLibrary.Emgu
{
  public static class ImageExtensions
  {
    public static void ClearROI(this Image<Bgr, byte> image) => image.ROI = Rectangle.Empty;
  }
}
