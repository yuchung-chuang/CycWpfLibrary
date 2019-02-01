using CycWpfLibrary.Input;
using CycWpfLibrary.Media;
using CycWpfLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    #region Dependency Properties
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(Pan),
      new PropertyMetadata(default(bool), OnIsEnabledChanged));
    [Category(AppNames.MVVM)]
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
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static MouseButton GetMouseButton(UIElement element)
      => (MouseButton)element.GetValue(MouseButtonProperty);
    public static void SetMouseButton(UIElement element, MouseButton value)
      => element.SetValue(MouseButtonProperty, value);
    #endregion

    private static readonly Cursor panCursor = ResourceManager.panCursor;
    private static Cursor cursorCache;
    private static bool isPanning;
    private static Point mouseAnchor;
    private static TranslateTransform translate;
    private static Point translateAnchor;

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement element))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached behavior on a UIElement.");

      if ((bool)e.NewValue)
      {
        element.MouseDown += Element_MouseDown;
        element.MouseUp += Element_MouseUp;
        element.MouseMove += Element_MouseMove;
        element.EnsureTransforms();
        element.Parent.SetValue(UIElement.ClipToBoundsProperty, true);
      }
      else
      {
        element.MouseDown -= Element_MouseDown;
        element.MouseUp -= Element_MouseUp;
        element.MouseMove -= Element_MouseMove;
      }
    }

    private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (e.IsMouseButtonPressed(GetMouseButton(element)))
      {
        var transforms = (element.RenderTransform as TransformGroup).Children;
        translate = transforms.GetTranslate();
        mouseAnchor = e.GetAbsolutePosition(element);
        translateAnchor = new Point(translate.X, translate.Y);
        cursorCache = element.Cursor;
        isPanning = true;
        element.Cursor = panCursor;
        element.CaptureMouse();
      }
    }
    private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (isPanning)
      {
        element.ReleaseMouseCapture();
        element.Cursor = cursorCache;
      }
    }
    private static void Element_MouseMove(object sender, MouseEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (element.IsMouseCaptured && e.IsMouseButtonPressed(GetMouseButton(element)))
      {
        var delta = e.GetAbsolutePosition(element) - mouseAnchor;
        var scale = (element.RenderTransform as TransformGroup).Children.GetScale();
        var toX = Math.Clamp(translateAnchor.X + delta.X, 0, element.ActualWidth * (1 - scale.ScaleX));
        var toY = Math.Clamp(translateAnchor.Y + delta.Y, 0, element.ActualHeight * (1 - scale.ScaleY));
        translate.AnimateTo(TranslateTransform.XProperty, toX, 0);
        translate.AnimateTo(TranslateTransform.YProperty, toY, 0);
      }

    }
  }
}
