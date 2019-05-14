using System.Windows;

namespace CycWpfLibrary
{
  public class ObservedWidth : AttachedPropertyBase<ObservedWidth, double> { }
  public class ObservedHeight : AttachedPropertyBase<ObservedHeight, double> { }
  public class IsObserveSize : AttachedPropertyBase<IsObserveSize, bool>
  {
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      base.OnValueChanged(sender, e);
      var frameworkElement = (FrameworkElement)sender;
      // remove any previous event
      frameworkElement.SizeChanged -= OnSizeChanged;
      // if user set IsObserveSize to true...
      if ((bool)e.NewValue)
      {
        // Start listening out for size changes
        frameworkElement.SizeChanged += OnSizeChanged;
        // Set default values
        UpdateObservedSizes(frameworkElement);
      }
    }

    private void UpdateObservedSizes(FrameworkElement frameworkElement)
    {
      ObservedWidth.SetValue(frameworkElement, frameworkElement.ActualWidth);
      ObservedHeight.SetValue(frameworkElement, frameworkElement.ActualHeight);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      UpdateObservedSizes((FrameworkElement)sender);
    }
  }
}
