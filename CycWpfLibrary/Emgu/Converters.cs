using CycWpfLibrary.WinForm;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CycWpfLibrary.Emgu
{
  public static class Converters
  {
    public static MCvScalar ToMCvScalar(this Color color) => new Bgr(color.ToWinForm()).MCvScalar;
  }
}
