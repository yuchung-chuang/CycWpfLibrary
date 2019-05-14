using System.ComponentModel;
using System.Windows;

namespace CycWpfLibrary
{
  public static class SizeObserver 
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
       "IsEnabled",
       typeof(bool),
       typeof(SizeObserver),
       new FrameworkPropertyMetadata(OnIsEnabledChanged));

    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static bool GetIsEnabled(DependencyObject obj)
        => (bool)obj.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(DependencyObject obj, bool value)
        => obj.SetValue(IsEnabledProperty, value);

    public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.RegisterAttached(
       "ActualWidth",
       typeof(double),
       typeof(SizeObserver),
       new FrameworkPropertyMetadata());
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static double GetActualWidth(DependencyObject obj)
        => (double)obj.GetValue(ActualWidthProperty);
    public static void SetActualWidth(DependencyObject obj, double value)
        => obj.SetValue(ActualWidthProperty, value);

    public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.RegisterAttached(
       "ActualHeight",
       typeof(double),
       typeof(SizeObserver),
       new FrameworkPropertyMetadata());
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static double GetActualHeight(DependencyObject obj)
        => (double)obj.GetValue(ActualHeightProperty);
    public static void SetActualHeight(DependencyObject obj, double value)
        => obj.SetValue(ActualHeightProperty, value);

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var frameworkElement = d as FrameworkElement;

      if ((bool)e.NewValue)
      {
        frameworkElement.SizeChanged += OnSizeChanged;
        UpdateSize(frameworkElement);
      }
      else
      {
        frameworkElement.SizeChanged -= OnSizeChanged;
      }
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      UpdateSize(sender as FrameworkElement);
    }

    private static void UpdateSize(FrameworkElement frameworkElement)
    {
      SetActualHeight(frameworkElement, frameworkElement.ActualHeight);
      SetActualWidth(frameworkElement, frameworkElement.ActualWidth);
    }
  }
}
