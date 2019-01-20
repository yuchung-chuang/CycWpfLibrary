using CycWpfLibrary.WinForm;
using Emgu.CV.Structure;
using System.Windows.Media;

namespace CycWpfLibrary.Emgu
{
  public static class ColorConverters
  {
    public static MCvScalar ToMCvScalar(this Color color) => new Bgra(color.B, color.G, color.R, color.A).MCvScalar;
    public static Rgba ToRgba(this Color color) => new Rgba(color.R, color.G, color.B, color.A);
    public static Bgra ToBgra(this Color color) => new Bgra(color.B, color.G, color.R, color.A);
  }
}
