using CycWpfLibrary.Emgu;
using CycWpfLibrary.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CycWpfLibrary.MVVM
{
  public static class Eraser
  {
    #region Dependency Properties
    public static DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(Eraser),
      new PropertyMetadata(default(bool), OnIsEnabledChanged));
    [AttachedPropertyBrowsableForType(typeof(Image))]
    public static bool GetIsEnabled(UIElement element)
      => (bool)element.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(UIElement element, bool value)
      => element.SetValue(IsEnabledProperty, value);

    public static DependencyProperty MouseButtonProperty = DependencyProperty.RegisterAttached(
      "MouseButton",
      typeof(MouseButton),
      typeof(Eraser),
      new PropertyMetadata(default(MouseButton)));
    [AttachedPropertyBrowsableForType(typeof(Image))]
    public static MouseButton GetMouseButton(UIElement element)
      => (MouseButton)element.GetValue(MouseButtonProperty);
    public static void SetMouseButton(UIElement element, MouseButton value)
      => element.SetValue(MouseButtonProperty, value);

    public static DependencyProperty EraserSizeProperty = DependencyProperty.RegisterAttached(
      "EraserSize",
      typeof(double),
      typeof(Eraser),
      new PropertyMetadata(10d));
    [AttachedPropertyBrowsableForType(typeof(Image))]
    public static double GetEraserSize(UIElement element)
      => (double)element.GetValue(EraserSizeProperty);
    public static void SetEraserSzie(UIElement element, double value)
      => element.SetValue(EraserSizeProperty, value);
    #endregion

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement element))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached property on a {typeof(UIElement)}.");

      if ((bool)e.NewValue)
      {
        element.MouseDown += Element_MouseDown;
        element.MouseUp += Element_MouseUp;
        element.MouseMove += Element_MouseMove;
        element.MouseEnter += Element_MouseEnter;
        element.MouseLeave += Element_MouseLeave;
        element.MouseWheel += Element_MouseWheel;
      }
      else
      {
        element.MouseDown -= Element_MouseDown;
        element.MouseUp -= Element_MouseUp;
        element.MouseMove -= Element_MouseMove;
        element.MouseEnter -= Element_MouseEnter;
        element.MouseLeave -= Element_MouseLeave;
        element.MouseWheel -= Element_MouseWheel;
      }
    }

    private static Cursor cursorCache;
    private static Rectangle rectCursor = new Rectangle
    {
      Fill = new SolidColorBrush(Color.FromArgb(50, 0, 255, 255)),
      Stroke = new SolidColorBrush(Colors.Black),
      IsHitTestVisible = false,
    };
    private static void Element_MouseEnter(object sender, MouseEventArgs e)
    {
      var element = sender as FrameworkElement;
      cursorCache = element.Cursor;
      UpdateCursor(element);
      Debug.WriteLine("!!");
    }
    private static void Element_MouseLeave(object sender, MouseEventArgs e)
    {
      var element = sender as FrameworkElement;
      element.Cursor = cursorCache;
    }
    private static void Element_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      var element = sender as FrameworkElement;
      UpdateCursor(element);
    }
    private static void UpdateCursor(FrameworkElement element)
    {
      var size = GetEraserSize(element);
      rectCursor.Width = size;
      rectCursor.Height = size;
      rectCursor.StrokeThickness = size / 10;
      element.Cursor = rectCursor.ToCursor(new Point(0.5, 0.5));
    }

    private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
    {
      (sender as FrameworkElement).CaptureMouse();
      Element_MouseMove(sender, e);
    }
    private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
    {
      (sender as FrameworkElement).ReleaseMouseCapture();
    }
    private static void Element_MouseMove(object sender, MouseEventArgs e)
    {
      var control = sender as Image;
      var image = (control.Source as BitmapSource).ToBitmap().ToImage<Bgra, byte>();
      var eraserSize = GetEraserSize(control);
      var mousePos = e.GetPosition(control);
      if (IsPressed() && control.IsMouseCaptured)
      {
        image.EraseImage(new Rect(mousePos, new Vector(eraserSize, eraserSize)).Minus((eraserSize / 2, eraserSize / 2)));
        control.SetValue(Image.SourceProperty, image.ToBitmapSource());
      }

      bool IsPressed()
      {
        switch (GetMouseButton(control))
        {
          case MouseButton.Left:
            return e.LeftButton == MouseButtonState.Pressed;
          case MouseButton.Middle:
            return e.MiddleButton == MouseButtonState.Pressed;
          case MouseButton.Right:
            return e.RightButton == MouseButtonState.Pressed;
          case MouseButton.XButton1:
            return e.XButton1 == MouseButtonState.Pressed;
          case MouseButton.XButton2:
            return e.XButton2 == MouseButtonState.Pressed;
          default:
            return false;
        }
      }
    }
  }
}
