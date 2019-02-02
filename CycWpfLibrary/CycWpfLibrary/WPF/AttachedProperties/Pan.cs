using CycWpfLibrary.Input;
using CycWpfLibrary.Media;
using CycWpfLibrary.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary
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

    public static readonly DependencyProperty InputsProperty = DependencyProperty.RegisterAttached(
        "Inputs",
        typeof(CycInputCollection),
        typeof(Pan),
        new PropertyMetadata());
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static CycInputCollection GetInputs(DependencyObject obj)
        => (CycInputCollection)obj.GetValue(InputsProperty);
    public static void SetInputs(DependencyObject obj, CycInputCollection value)
        => obj.SetValue(InputsProperty, value);

    public static readonly DependencyProperty InputProperty = DependencyProperty.RegisterAttached(
        "Input",
        typeof(CycInput),
        typeof(Pan),
        new PropertyMetadata(default(CycInput)));
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    [TypeConverter(typeof(CycInputTypeConverter))]
    public static CycInput GetInput(DependencyObject obj)
        => (CycInput)obj.GetValue(InputProperty);
    public static void SetInput(DependencyObject obj, CycInput value)
        => obj.SetValue(InputProperty, value);
    #endregion

    private static readonly Cursor panCursor = ResourceManager.panCursor;
    private static Cursor cursorCache;
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
      if (InputCheck(element))
      {
        var transforms = (element.RenderTransform as TransformGroup).Children;
        translate = transforms.GetTranslate();
        mouseAnchor = e.GetAbsolutePosition(element);
        translateAnchor = new Point(translate.X, translate.Y);
        cursorCache = element.Cursor;
        element.Cursor = panCursor;
        element.CaptureMouse();
      }
    }
    private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (element.IsMouseCaptured)
      {
        element.ReleaseMouseCapture();
        element.Cursor = cursorCache;
      }
    }
    private static void Element_MouseMove(object sender, MouseEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (element.IsMouseCaptured && InputCheck(element))
      {
        var delta = e.GetAbsolutePosition(element) - mouseAnchor;
        var scale = (element.RenderTransform as TransformGroup).Children.GetScale();
        var toX = Math.Clamp(translateAnchor.X + delta.X, 0, element.ActualWidth * (1 - scale.ScaleX));
        var toY = Math.Clamp(translateAnchor.Y + delta.Y, 0, element.ActualHeight * (1 - scale.ScaleY));
        translate.AnimateTo(TranslateTransform.XProperty, toX, 0);
        translate.AnimateTo(TranslateTransform.YProperty, toY, 0);
      }

    }

    private static bool InputCheck(FrameworkElement element)
    {
      var inputs = GetInputs(element) ?? new CycInputCollection();
      var input = GetInput(element) ?? new CycInput();
      return (!input.IsEmpty && input.IsValid) || (!inputs.IsEmpty && inputs.IsValid) ? true : false;
    }
  }
}
