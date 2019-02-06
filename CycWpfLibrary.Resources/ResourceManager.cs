using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary.Resources
{
  public class ResourceManager
  {
    public ResourceManager()
    {

    }

    public static readonly Cursor PanCursor = new Cursor(Application.GetResourceStream(new Uri(@"pack://application:,,,/CycWpfLibrary.Resources;component/pan.cur", UriKind.Absolute)).Stream);

    public static readonly DrawingBrush CrossboardBrush = new DrawingBrush
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

    public static readonly SolidColorBrush ShadowBrush = new SolidColorBrush(Color.FromArgb((byte)(255 * 0.3), 0, 0, 0));
  }
}
