using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary.Controls
{
  public class PanZoomBorder : Border
  {
    private FrameworkElement child = null;
    private Point origin;
    private Point start;

    public TranslateTransform tt;
    public ScaleTransform st;

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
      this.child = element as FrameworkElement;
      if (child != null)
      {
        TransformGroup group = new TransformGroup();
        st = new ScaleTransform();
        group.Children.Add(st);
        tt = new TranslateTransform();
        group.Children.Add(tt);
        child.RenderTransform = group;
        child.RenderTransformOrigin = new Point(0, 0);
        MouseWheel += child_MouseWheel;
        MouseLeftButtonDown += child_MouseLeftButtonDown;
        MouseLeftButtonUp += child_MouseLeftButtonUp;
        MouseMove += child_MouseMove;
        PreviewMouseRightButtonDown += new MouseButtonEventHandler(
          child_PreviewMouseRightButtonDown);
      }
    }

    public void Reset()
    {
      if (child != null)
      {
        st.ScaleX = 1.0;
        st.ScaleY = 1.0;
        tt.X = 0.0;
        tt.Y = 0.0;
      }
    }

    #region Child Events

    private void child_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (child != null)
      {
        double zoom = e.Delta > 0 ? .2 : -.2;
        if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
          return;

        var relative = e.GetPosition(child);
        var absolute = e.GetAbsolutePosition(child);
        
        st.ScaleX += zoom;
        st.ScaleY += zoom;

        tt.X = absolute.X - relative.X * st.ScaleX;
        tt.Y = absolute.Y - relative.Y * st.ScaleY;
      }
    }

    private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (child != null)
      {
        start = e.GetPosition(this);
        origin = new Point(tt.X, tt.Y);
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

    void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Reset();
    }

    private void child_MouseMove(object sender, MouseEventArgs e)
    {
      if (child != null)
      {
        if (child.IsMouseCaptured)
        {
          Vector v = e.GetPosition(this) - start;
          tt.X = origin.X + v.X;
          tt.Y = origin.Y + v.Y;
          
        }
      }
    }

    #endregion
  }
}