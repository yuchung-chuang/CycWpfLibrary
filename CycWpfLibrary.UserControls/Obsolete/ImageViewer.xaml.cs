using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CycWpfLibrary.UserControls
{
  [Obsolete("Content in UserControl cannot be named.")]
  public partial class ImageViewer : UserControl
  {
    public ImageViewer()
    {
      InitializeComponent();
    }
    
    private Point origin;
    private Point start;

    private UIElement ContentElement => Content as UIElement;
    //private bool HasContent => Content != null;
    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      AssignTransforms();
    }

    private TranslateTransform GetTranslateTransform()
    {
      return (TranslateTransform)((TransformGroup)ContentElement.RenderTransform).Children.First(tr => tr is TranslateTransform);
    }
    private ScaleTransform GetScaleTransform()
    {
      return (ScaleTransform)((TransformGroup)ContentElement.RenderTransform).Children.First(tr => tr is ScaleTransform);
    }
    private TranslateTransform GetTranslateTransform(UIElement element)
    {
      return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform);
    }
    private ScaleTransform GetScaleTransform(UIElement element)
    {
      return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform);
    }

    public void AssignTransforms()
    {
      if (HasContent)
      {
        TransformGroup group = new TransformGroup();
        ScaleTransform st = new ScaleTransform();
        TranslateTransform tt = new TranslateTransform();
        group.Children.Add(st);
        group.Children.Add(tt);
        ContentElement.RenderTransform = group;
        ContentElement.RenderTransformOrigin = new Point(0.0, 0.0);
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
      if (HasContent)
      {
        // reset zoom
        var st = GetScaleTransform();
        st.ScaleX = 1.0;
        st.ScaleY = 1.0;

        // reset pan
        var tt = GetTranslateTransform();
        tt.X = 0.0;
        tt.Y = 0.0;
      }
    }

    #region Child Events
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      base.OnMouseWheel(e);

    }

    private void child_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (HasContent)
      {
        var st = GetScaleTransform();
        var tt = GetTranslateTransform();

        double zoom = e.Delta > 0 ? .2 : -.2;
        if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
          return;

        Point relative = e.GetPosition(ContentElement);
        double abosuluteX;
        double abosuluteY;

        abosuluteX = relative.X * st.ScaleX + tt.X;
        abosuluteY = relative.Y * st.ScaleY + tt.Y;

        st.ScaleX += zoom;
        st.ScaleY += zoom;

        tt.X = abosuluteX - relative.X * st.ScaleX;
        tt.Y = abosuluteY - relative.Y * st.ScaleY;
      }
    }

    private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (HasContent)
      {
        var tt = GetTranslateTransform();
        start = e.GetPosition(this);
        origin = new Point(tt.X, tt.Y);
        Cursor = new Cursor(Application.GetResourceStream(new Uri(@"/CycWpfLibrary;component/Controls/Resources/cursor.cur", UriKind.RelativeOrAbsolute)).Stream);
        ContentElement.CaptureMouse();
      }
    }

    private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (HasContent)
      {
        ContentElement.ReleaseMouseCapture();
        this.Cursor = Cursors.Arrow;
      }
    }

    private void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Reset();
    }

    private void child_MouseMove(object sender, MouseEventArgs e)
    {
      if (HasContent)
      {
        if (ContentElement.IsMouseCaptured)
        {
          var tt = GetTranslateTransform();
          Vector v = start - e.GetPosition(this);
          tt.X = origin.X - v.X;
          tt.Y = origin.Y - v.Y;
        }
      }
    }

    #endregion
  }
}
