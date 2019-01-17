using CycWpfLibrary.Media;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Math;


namespace CycWpfLibrary.MVVM
{
  public static class Zoom
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(Zoom),
        new PropertyMetadata(OnIsEnabledChanged));

    [AttachedPropertyBrowsableForType(typeof(UIElement))]
    public static bool GetIsEnabled(UIElement obj)
    {
      return (bool)obj.GetValue(IsEnabledProperty);
    }
    public static void SetIsEnabled(UIElement obj, bool value)
    {
      obj.SetValue(IsEnabledProperty, value);
    }

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement element))
        throw new NotSupportedException("Can only set the Zoom.EnableMouseWheel attached behavior on a UIElement.");

      if ((bool)e.NewValue)
      {
        element.PreviewMouseWheel += OnMouseWheel;
        EnsureRenderTransform(element);
        element.RenderTransformOrigin = new Point(0, 0);
        element.Parent.SetValue(UIElement.ClipToBoundsProperty, true);
      }
      else
      {
        element.PreviewMouseWheel -= OnMouseWheel;

      }
    }

    private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
      var element = sender as UIElement;
      var transforms = (element.RenderTransform as TransformGroup).Children;
      var translate = transforms.GetTranslate();
      var scale = transforms.GetScale();
      if (GetIsEnabled(element))
      {
        double zoom = e.Delta > 0 ? .2 : -.2;
        if (!(e.Delta > 0) && (scale.ScaleX < .4 || scale.ScaleY < .4))
          return;

        var relative = e.GetPosition(element);
        var absolute = e.GetAbsolutePosition(element);
        //必須是scale先，translate後
        scale.ScaleX += zoom;
        scale.ScaleY += zoom;
        translate.X = absolute.X - relative.X * scale.ScaleX;
        translate.Y = absolute.Y - relative.Y * scale.ScaleY;
      }
    }

    private static void EnsureRenderTransform(UIElement element)
    {
      element.RenderTransform = new TransformGroup
      {
        Children = new TransformCollection
        {
          // 必須是scale先，translate後
          new ScaleTransform(),
          new TranslateTransform(),
        },
      };
      element.RenderTransformOrigin = new Point(0, 0);
    }
  }
}
