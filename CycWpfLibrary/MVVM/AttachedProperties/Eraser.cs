using CycWpfLibrary.Emgu;
using CycWpfLibrary.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorWpf = System.Windows.Media.Color;
using ImageControl = System.Windows.Controls.Image;
using PointWpf = System.Windows.Point;
using RectangleWpf = System.Windows.Shapes.Rectangle;

namespace CycWpfLibrary.MVVM
{
  public class EraserCache : ObservableDependencyObject
  {
    public Image<Bgra, byte> Image { get; set; }

    public Cursor Cache;
    public double Size;

    public BitmapSource ImageSource => Image?.ToBitmapSource();

    public FrameworkElement Cursor = new RectangleWpf
    {
      Fill = new SolidColorBrush(ColorWpf.FromArgb(50, 0, 255, 255)),
      Stroke = new SolidColorBrush(Colors.Black),
    };

    public ScaleTransform Scale;
  }

  /// <summary>
  /// Add CursorProperty, CommandProperty
  /// </summary>
  public static class Eraser
  {
    #region Public Dependency Properties
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
      typeof(bool),
      typeof(Eraser),
      new PropertyMetadata(default(bool), OnIsEnabledChanged));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static bool GetIsEnabled(UIElement element)
      => (bool)element.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(UIElement element, bool value)
      => element.SetValue(IsEnabledProperty, value);

    public static readonly DependencyProperty MouseButtonProperty = DependencyProperty.RegisterAttached(
      "MouseButton",
      typeof(MouseButton),
      typeof(Eraser),
      new PropertyMetadata(default(MouseButton)));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static MouseButton GetMouseButton(UIElement element)
      => (MouseButton)element.GetValue(MouseButtonProperty);
    public static void SetMouseButton(UIElement element, MouseButton value)
      => element.SetValue(MouseButtonProperty, value);

    public static readonly DependencyProperty EraserSizeProperty = DependencyProperty.RegisterAttached(
      "EraserSize",
      typeof(double),
      typeof(Eraser),
      new PropertyMetadata(10d));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static double GetEraserSize(UIElement element)
      => (double)element.GetValue(EraserSizeProperty);
    public static void SetEraserSzie(UIElement element, double value)
      => element.SetValue(EraserSizeProperty, value);

    public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached(
        "Image",
        typeof(Image<Bgra, byte>),
        typeof(Eraser),
        new PropertyMetadata(default(Image<Bgra, byte>), OnImageChanged));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static Image<Bgra, byte> GetImage(DependencyObject obj)
        => (Image<Bgra, byte>)obj.GetValue(ImageProperty);
    public static void SetImage(DependencyObject obj, Image<Bgra, byte> value)
        => obj.SetValue(ImageProperty, value);

    public static readonly DependencyProperty MouseDownCommandProperty = DependencyProperty.RegisterAttached(
        "MouseDownCommand",
        typeof(ICommand),
        typeof(Eraser),
        new PropertyMetadata(default(ICommand)));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static ICommand GetMouseDownCommand(DependencyObject obj)
        => (ICommand)obj.GetValue(MouseDownCommandProperty);
    public static void SetMouseDownCommand(DependencyObject obj, ICommand value)
        => obj.SetValue(MouseDownCommandProperty, value);

    public static readonly DependencyProperty CursorProperty = DependencyProperty.RegisterAttached(
        "Cursor",
        typeof(FrameworkElement),
        typeof(Eraser),
        new PropertyMetadata(default(FrameworkElement), OnCursorChanged));
    [AttachedPropertyBrowsableForType(typeof(ImageControl))]
    public static FrameworkElement GetCursor(DependencyObject obj)
        => (FrameworkElement)obj.GetValue(CursorProperty);
    public static void SetCursor(DependencyObject obj, FrameworkElement value)
        => obj.SetValue(CursorProperty, value);
    #endregion

    #region Private Dependency Properties
    public static readonly DependencyProperty EraserCacheProperty = DependencyProperty.RegisterAttached(
        "EraserCache2",
        typeof(EraserCache),
        typeof(Eraser),
        new PropertyMetadata(default(EraserCache)));
    private static EraserCache GetEraserCache(DependencyObject obj)
        => (EraserCache)obj.GetValue(EraserCacheProperty);
    private static void SetEraserCache(DependencyObject obj, EraserCache value)
        => obj.SetValue(EraserCacheProperty, value);
    #endregion

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageControl control))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached property on a {typeof(ImageControl)}.");

      if ((bool)e.NewValue)
      {
        control.MouseDown += Element_MouseDown;
        control.MouseUp += Element_MouseUp;
        control.MouseMove += Element_MouseMove;
        control.MouseEnter += Element_MouseEnter;
        control.MouseLeave += Element_MouseLeave;
        control.MouseWheel += Element_MouseWheel;

        Initialize(control);
        var eraserCache = GetEraserCache(control);
        control.SetBinding(ImageControl.SourceProperty, new Binding
        {
          Source = eraserCache,
          Path = new PropertyPath(nameof(eraserCache.ImageSource)),
          Mode = BindingMode.OneWay,
        });
      }
      else
      {
        control.MouseDown -= Element_MouseDown;
        control.MouseUp -= Element_MouseUp;
        control.MouseMove -= Element_MouseMove;
        control.MouseEnter -= Element_MouseEnter;
        control.MouseLeave -= Element_MouseLeave;
        control.MouseWheel -= Element_MouseWheel;
      }
    }
    private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageControl control))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached property on a {typeof(ImageControl)}.");
      if (!GetIsEnabled(control))
        throw new InvalidOperationException($"To use {typeof(Eraser)}, {IsEnabledProperty} should be set as true.");

      Initialize(control);
    }
    private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageControl control))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached property on a {typeof(ImageControl)}.");
      if (!GetIsEnabled(control))
        throw new InvalidOperationException($"To use {typeof(Eraser)}, {IsEnabledProperty} should be set as true.");

      var eraserCache = GetEraserCache(control);
      eraserCache.Image = GetImage(control);
    }

    private static void Initialize(ImageControl control)
    {
      var eraserCache = GetEraserCache(control) ?? new EraserCache();
      var cursor = GetCursor(control) ?? eraserCache.Cursor;
      control.EnsureTransforms();
      eraserCache.Scale = (control.RenderTransform as TransformGroup).Children.GetScale();
      cursor.IsHitTestVisible = false;
      cursor.Visibility = Visibility.Collapsed;
      eraserCache.Cursor = cursor;
      SetEraserCache(control, eraserCache);
    }

    private static void Element_MouseEnter(object sender, MouseEventArgs e)
    {
      var control = sender as ImageControl;
      var eraserCache = GetEraserCache(control);
      eraserCache.Cache = control.Cursor;
      UpdateCursor(control);
    }
    private static void Element_MouseLeave(object sender, MouseEventArgs e)
    {
      var control = sender as ImageControl;
      var eraserCache = GetEraserCache(control);
      control.Cursor = eraserCache.Cache;
    }
    private static void Element_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      var control = sender as ImageControl;
      UpdateCursor(control);
    }
    /// <summary>
    /// Size different???? Color different????
    /// </summary>
    private static void UpdateCursor(ImageControl control)
    {
      var eraserCache = GetEraserCache(control);
      eraserCache.Size = GetEraserSize(control) / eraserCache.Scale.ScaleX;
      eraserCache.Cursor.Width = eraserCache.Size;
      eraserCache.Cursor.Height = eraserCache.Size;
      eraserCache.Cursor.Visibility = Visibility.Visible;
      control.Cursor = eraserCache.Cursor.ToCursor(new PointWpf(0.5, 0.5));
      eraserCache.Cursor.Visibility = Visibility.Hidden;
    }

    private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
    {
      (sender as ImageControl).CaptureMouse();
      Element_MouseMove(sender, e);
    }
    private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
    {
      (sender as ImageControl).ReleaseMouseCapture();
    }
    private static void Element_MouseMove(object sender, MouseEventArgs e)
    {
      var control = sender as ImageControl;
      if (IsPressed() && control.IsMouseCaptured)
      {
        var eraseCache2 = GetEraserCache(control);
        var mousePos = e.GetPosition(control);
        eraseCache2.Image = eraseCache2.Image.EraseImage(new Rect(mousePos.Minus(new PointWpf(eraseCache2.Size / 2, eraseCache2.Size / 2)), new Vector(eraseCache2.Size, eraseCache2.Size)));
        //eraseCache2.Image = eraseCache2.Image.Clone(); // update imageSource
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
