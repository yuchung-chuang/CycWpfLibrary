using System.Windows;

namespace CycWpfLibrary
{
  public static class ApplicationExtensions
  {
    /// <summary>
    /// Recursively get window of input <paramref name="element"/>
    /// </summary>
    public static Window GetWindow(this FrameworkElement element)
    {
      if (element is Window window)
        return window;
      else
        return (element.Parent as FrameworkElement).GetWindow();
    }
  }
}
