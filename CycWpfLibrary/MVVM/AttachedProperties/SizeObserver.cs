using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.MVVM
{
  public class ObservedWidthProperty : AttachedPropertyBase<ObservedWidthProperty, double>
  {
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      base.OnValueChanged(sender, e);

      SizeObserver.OnSizeChanged(sender, e);
    }
  }

  public class ObservedHeightProperty : AttachedPropertyBase<ObservedHeightProperty, double>
  {
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      base.OnValueChanged(sender, e);

      SizeObserver.OnSizeChanged(sender, e);
    }
  }

  public static class SizeObserver
  {
    public static void OnSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var frameworkElement = sender as FrameworkElement;

      if ((bool)e.NewValue)
      {
        frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
        UpdateObservedSizesForFrameworkElement(frameworkElement);
      }
      else
      {
        frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
      }
    }

    public static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
    {
      UpdateObservedSizesForFrameworkElement(sender as FrameworkElement);
    }

    public static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
    {
      ObservedWidthProperty.SetValue(frameworkElement, frameworkElement.ActualWidth);
      ObservedHeightProperty.SetValue(frameworkElement, frameworkElement.ActualHeight);
    }
  }
}
