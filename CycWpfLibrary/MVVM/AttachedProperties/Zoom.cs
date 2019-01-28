using CycWpfLibrary.Input;
using CycWpfLibrary.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Math;


namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供縮放相依屬性的靜態類別。
  /// </summary>
  public static class Zoom
  {
    #region Dependency Properties
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(Zoom),
      new PropertyMetadata(OnIsEnabledChanged));
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static bool GetIsEnabled(UIElement element)
      => (bool)element.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(UIElement element, bool value)
      => element.SetValue(IsEnabledProperty, value);
    
    public static readonly DependencyProperty IsLeaveResetProperty = DependencyProperty.RegisterAttached(
      "IsLeaveReset",
      typeof(bool),
      typeof(Zoom),
      new PropertyMetadata(default(bool)));
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static bool GetIsLeaveReset(UIElement element)
      => (bool)element.GetValue(IsLeaveResetProperty);
    public static void SetIsLeaveReset(UIElement element, bool value)
      => element.SetValue(IsLeaveResetProperty, value);

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.RegisterAttached(
      "Maximum",
      typeof(double),
      typeof(Zoom),
      new PropertyMetadata(5d, OnMaximumChanged));
    [Category(AppNames.MVVM)]
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static double GetMaximum(UIElement element)
      => (double)element.GetValue(MaximumProperty);
    public static void SetMaximum(UIElement element, double value)
      => element.SetValue(MaximumProperty, value);
    #endregion

    private static readonly DrawingBrush background = new DrawingBrush
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

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement element))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached behavior on a UIElement.");

      if ((bool)e.NewValue)
      {
        element.MouseWheel += Element_MouseWheel;
        if (GetIsLeaveReset(element))
          element.MouseLeave += Element_MouseLeave;
        element.EnsureTransforms();
        element.RenderTransformOrigin = new Point(0, 0);
        element.Parent.SetValue(UIElement.ClipToBoundsProperty, true);
        if (element.Parent is Panel parent)
          parent.Background = background;
        else if (element.Parent is Border border)
          border.Background = background;
      }
      else
      {
        element.MouseWheel -= Element_MouseWheel;
        element.MouseLeave -= Element_MouseLeave;
      }
    }

    private static double LeaveTime = 1;
    private static double WheelTime = 0.1;

    private static void Element_MouseLeave(object sender, MouseEventArgs e)
    {
      var element = sender as UIElement;
      var transforms = (element.RenderTransform as TransformGroup).Children;
      var translate = transforms.GetTranslate();
      var scale = transforms.GetScale();
      translate.AnimateTo(TranslateTransform.XProperty, 1d, LeaveTime);
      translate.AnimateTo(TranslateTransform.YProperty, 1d, LeaveTime);
      scale.AnimateTo(ScaleTransform.ScaleXProperty, 1d, LeaveTime);
      scale.AnimateTo(ScaleTransform.ScaleYProperty, 1d, LeaveTime);
    }
    private static void Element_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      var element = sender as FrameworkElement;
      var parent = element.Parent as FrameworkElement;
      var transforms = (element.RenderTransform as TransformGroup).Children;
      var translate = transforms.GetTranslate();
      var scale = transforms.GetScale();

      //ZoomSpeed
      double zoom = e.Delta > 0 ? .2 : -.2;

      var relative = e.GetPosition(element);
      var absolute = e.GetAbsolutePosition(element);
      //必須是scale先，translate後
      var ToScale = Math.Clamp(scale.ScaleX + zoom, GetMaximum(element), 1);
      var ToX = Math.Clamp(absolute.X - relative.X * ToScale, 0, element.ActualWidth * (1 - ToScale));
      var ToY = Math.Clamp(absolute.Y - relative.Y * ToScale, 0, element.ActualHeight * (1 - ToScale));
      scale.AnimateTo(ScaleTransform.ScaleXProperty, ToScale, WheelTime);
      scale.AnimateTo(ScaleTransform.ScaleYProperty, ToScale, WheelTime);
      translate.AnimateTo(TranslateTransform.XProperty, ToX, WheelTime);
      translate.AnimateTo(TranslateTransform.YProperty, ToY, WheelTime);

    }
    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var value = (double)e.NewValue;
      if (value < 1)
      {
        throw new InvalidOperationException($"{MaximumProperty} must be greater than inital scale (1).");
      }
    }
  }
}
