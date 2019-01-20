using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CycWpfLibrary.Media;

namespace CycWpfLibrary.Controls
{
  public class PanZoomBorder : Border
  {
    private FrameworkElement child = null;
    private Point origin;
    private Point start;

    private TranslateTransform translate;
    public TranslateTransform Translate
    {
      get => translate;
      set
      {
        translate.X = Math.Clamp(value.X, 0, child.ActualWidth * (1 - scale.ScaleX));
        translate.Y = Math.Clamp(value.Y, 0, child.ActualHeight * (1 - scale.ScaleY));
      }
    }
    public ScaleTransform scale { get; set; }
    public bool IsEnableWheel { get; set; } = true;
    public bool IsEnableDrag { get; set; } = true;

    public override UIElement Child
    {
      get { return base.Child; }
      set
      {
        if (value != null && value != this.Child)
          this.Initialize(value);
        base.Child = value;
      }
    }


    public void Initialize(UIElement element)
    {
      child = element as FrameworkElement;
      if (child != null)
      {
        child.EnsureTransforms();
        var transforms = (child.RenderTransform as TransformGroup).Children;
        translate = transforms.GetTranslate();
        scale = transforms.GetScale();
        MouseWheel += child_MouseWheel;
        MouseLeftButtonDown += child_MouseLeftButtonDown;
        MouseLeftButtonUp += child_MouseLeftButtonUp;
        MouseMove += child_MouseMove;
      }
    }

    public void Reset()
    {
      if (child != null)
      {
        scale.ScaleX = 1.0;
        scale.ScaleY = 1.0;
        Translate.X = 0.0;
        Translate.Y = 0.0;
      }
    }

    #region Child Events

    private void child_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (child != null && IsEnableWheel && !isDraging)
      {
        double zoom = e.Delta > 0 ? .2 : -.2;
        if (!(e.Delta > 0) && (scale.ScaleX < .4 || scale.ScaleY < .4))
          return;

        var relative = e.GetPosition(child);
        var absolute = e.GetAbsolutePosition(child);
        //必須是scale先，translate後
        scale.ScaleX += zoom;
        scale.ScaleY += zoom;
        var translate = new TranslateTransform
        {
          X = absolute.X - relative.X * scale.ScaleX,
          Y = absolute.Y - relative.Y * scale.ScaleY,
        };
        Translate = translate;
      }
    }

    private bool isDraging = false;
    private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (child != null)
      {
        start = e.GetPosition(this);
        origin = new Point(Translate.X, Translate.Y);
        Cursor = new Cursor(Application.GetResourceStream(new Uri(@"/CycWpfLibrary;component/Controls/Resources/cursor.cur", UriKind.RelativeOrAbsolute)).Stream);
        child.CaptureMouse();
      }
    }

    private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (child != null)
      {
        child.ReleaseMouseCapture();
        this.Cursor = Cursors.Arrow;
      }
    }

    private void child_MouseMove(object sender, MouseEventArgs e)
    {
      if (child != null && child.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
      {
        e.Handled = true;
        Vector v = e.GetPosition(this) - start;
        var translate = new TranslateTransform
        {
          X = origin.X + v.X,
          Y = origin.Y + v.Y,
        };
        Translate = translate;
      }
      
    }

    #endregion
  }
}