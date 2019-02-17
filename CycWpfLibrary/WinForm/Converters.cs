using RectWinForm = System.Drawing.Rectangle;
using RectWpf = System.Windows.Rect;
using PointWinForm = System.Drawing.Point;
using PointWpf = System.Windows.Point;
using SizeWinForm = System.Drawing.Size;
using SizeWpf = System.Windows.Size;
using ColorWinForm = System.Drawing.Color;
using ColorWpf = System.Windows.Media.Color;

namespace CycWpfLibrary
{
  public static class Converters
  {
    public static PointWpf ToWpf(this PointWinForm point) => new PointWpf(point.X, point.Y);
    public static PointWinForm ToWinForm(this PointWpf point) => new PointWinForm((int)point.X, (int)point.Y);

    public static SizeWpf ToWpf(this SizeWinForm size) => new SizeWpf(size.Width, size.Height);
    public static SizeWinForm ToWinForm(this SizeWpf size) => new SizeWinForm((int)size.Width, (int)size.Height);

    public static RectWinForm ToWinForm(this RectWpf rect) => new RectWinForm(rect.Location.ToWinForm(), rect.Size.ToWinForm());
    public static RectWpf ToWpf(this RectWinForm rectangle) => new RectWpf(rectangle.Location.ToWpf(), rectangle.Size.ToWpf());

    public static ColorWinForm ToWinForm(this ColorWpf color) => ColorWinForm.FromArgb(color.A, color.R, color.G, color.B);
    public static ColorWpf ToWpf(this ColorWinForm color) => ColorWpf.FromArgb(color.A, color.R, color.G, color.B);

  }
}
