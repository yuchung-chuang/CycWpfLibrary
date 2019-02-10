using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.Resources
{
  public class CycResources
  {
    public CycResources() { } // for xaml initialization

    /// <summary>
    /// Usage: <see cref="PackUri"/> + "AssemblyName;component/fileName"
    /// </summary>
    public static string PackUri { get; } = @"pack://application:,,,/";

    public static string CurrentPackUri { get; } = PackUri + @"CycWpfLibrary.Resources;component/";

    public static Cursor PanCursor { get; } = new Cursor(Application.GetResourceStream(new Uri(CurrentPackUri + "pan.cur", UriKind.Absolute)).Stream);

    public static DrawingBrush CrossboardBrush { get; } = new DrawingBrush
    {
      TileMode = TileMode.Tile,
      Viewport = new Rect(0, 0, 32, 32),
      ViewportUnits = BrushMappingMode.Absolute,
      Drawing = new GeometryDrawing
      {
        Brush = Brushes.LightGray,
        Geometry = Geometry.Parse("M0,0 H16 V16 H32 V32 H16 V16 H0Z"),
      }
    };

    public static SolidColorBrush ShadowBrush { get; } = new SolidColorBrush(Color.FromArgb((byte)(255 * 0.3), 0, 0, 0));

    public static SolidColorBrush DarkShadowBrush { get; } = new SolidColorBrush(Color.FromArgb((byte)(255 * 0.7), 0, 0, 0));

    public static Uri MouseLeftButtonUri { get; } = new Uri(CurrentPackUri + "MouseLeftButton.png");
    public static Uri MouseRightButtonUri { get; } = new Uri(CurrentPackUri + "MouseRightButton.png");
    public static Uri MouseWheelUri { get; } = new Uri(CurrentPackUri + "MouseWheel.png");
  }
}
