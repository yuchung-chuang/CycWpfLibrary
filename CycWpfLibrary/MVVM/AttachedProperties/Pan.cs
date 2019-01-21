using CycWpfLibrary.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供平移相依屬性的靜態類別。
  /// </summary>
  public static class Pan
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(Pan),
      new PropertyMetadata(default(bool), OnIsEnabledChanged));
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static bool GetIsEnabled(UIElement element)
      => (bool)element.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(UIElement element, bool value)
      => element.SetValue(IsEnabledProperty, value);

    public static readonly DependencyProperty MouseButtonProperty = DependencyProperty.RegisterAttached(
      "MouseButton",
      typeof(MouseButton),
      typeof(Pan),
      new PropertyMetadata(default(MouseButton)));
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static MouseButton GetMouseButton(UIElement element)
      => (MouseButton)element.GetValue(MouseButtonProperty);
    public static void SetMouseButton(UIElement element, MouseButton value)
      => element.SetValue(MouseButtonProperty, value);

    public static readonly DependencyProperty MouseAnchorProperty = DependencyProperty.RegisterAttached(
      "MouseAnchor",
      typeof(Point),
      typeof(Pan),
      new PropertyMetadata(default(Point)));
    private static Point GetMouseAnchor(UIElement element)
      => (Point)element.GetValue(MouseAnchorProperty);
    private static void SetMouseAnchor(UIElement element, Point value)
      => element.SetValue(MouseAnchorProperty, value);

    public static readonly DependencyProperty TranslateAnchorProperty = DependencyProperty.RegisterAttached(
      "TranslateAnchor",
      typeof(Point),
      typeof(Pan),
      new PropertyMetadata(default(Point)));
    private static Point GetTranslateAnchor(UIElement element)
      => (Point)element.GetValue(TranslateAnchorProperty);
    private static void SetTranslateAnchor(UIElement element, Point value)
      => element.SetValue(TranslateAnchorProperty, value);

    public static readonly DependencyProperty TranslateProperty = DependencyProperty.RegisterAttached(
      "Translate",
      typeof(TranslateTransform),
      typeof(Pan),
      new PropertyMetadata(default(TranslateTransform)));
    private static TranslateTransform GetTranslate(UIElement element)
      => (TranslateTransform)element.GetValue(TranslateProperty);
    private static void SetTranslate(UIElement element, TranslateTransform value)
      => element.SetValue(TranslateProperty, value);

    public static readonly DependencyProperty IsPanningProperty = DependencyProperty.RegisterAttached(
      "IsPanning",
      typeof(bool),
      typeof(Pan),
      new PropertyMetadata(default(bool)));
    private static bool GetIsPanning(UIElement element)
      => (bool)element.GetValue(IsPanningProperty);
    private static void SetIsPanning(UIElement element, bool value)
      => element.SetValue(IsPanningProperty, value);

    public static readonly DependencyProperty CursorCacheProperty = DependencyProperty.RegisterAttached(
      "CursorCache",
      typeof(Cursor),
      typeof(Pan),
      new PropertyMetadata(default(Cursor)));
    private static Cursor GetCursorCache(UIElement element)
      => (Cursor)element.GetValue(CursorCacheProperty);
    private static void SetCursorCache(UIElement element, Cursor value)
      => element.SetValue(CursorCacheProperty, value);

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement element))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached behavior on a UIElement.");

      if ((bool)e.NewValue)
      {
        element.PreviewMouseDown += Element_PreviewMouseDown;
        element.PreviewMouseUp += Element_PreviewMouseUp;
        element.PreviewMouseMove += Element_PreviewMouseMove;
        element.EnsureTransforms();
        element.Parent.SetValue(UIElement.ClipToBoundsProperty, true);
      }
      else
      {
        element.PreviewMouseDown -= Element_PreviewMouseDown;
        element.PreviewMouseUp -= Element_PreviewMouseUp;
        element.PreviewMouseMove -= Element_PreviewMouseMove;

      }
    }

    private static Cursor panCursor = new Cursor(Application.GetResourceStream(new Uri(@"/CycWpfLibrary;component/Controls/Resources/pan.cur", UriKind.RelativeOrAbsolute)).Stream);

    private static void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (IsMouseButtonPressed(element, e))
      {
        var transforms = (element.RenderTransform as TransformGroup).Children;
        var translate = transforms.GetTranslate();
        SetTranslate(element, translate);
        SetMouseAnchor(element, e.GetAbsolutePosition(element));
        SetTranslateAnchor(element, new Point(translate.X, translate.Y));
        SetCursorCache(element, element.Cursor);
        element.Cursor = panCursor;
        element.CaptureMouse();
        SetIsPanning(element, true);
      }
    }
    private static void Element_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (GetIsPanning(element))
      {
        element.ReleaseMouseCapture();
        element.Cursor = GetCursorCache(element);
      }
    }
    private static void Element_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (element.IsMouseCaptured && IsMouseButtonPressed(element, e))
      {
        var delta = e.GetAbsolutePosition(element) - GetMouseAnchor(element);
        var scale = (element.RenderTransform as TransformGroup).Children.GetScale();
        var translate = GetTranslate(element);
        var translateAnchor = GetTranslateAnchor(element);
        var toX = Math.Clamp(translateAnchor.X + delta.X, 0, element.ActualWidth * (1 - scale.ScaleX));
        var toY = Math.Clamp(translateAnchor.Y + delta.Y, 0, element.ActualHeight * (1 - scale.ScaleY));
        translate.AnimateTo(TranslateTransform.XProperty, toX, 0);
        translate.AnimateTo(TranslateTransform.YProperty, toY, 0);
      }

    }

    private static bool IsMouseButtonPressed(UIElement element, MouseEventArgs e)
    {
      switch (GetMouseButton(element))
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
