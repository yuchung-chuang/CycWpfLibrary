using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public static class UIElementExtensions
  {
    public static void RemoveAll(this UIElementCollection collection, UIElement[] elements)
    {
      var n = elements?.Length ?? 0;
      for (int i = 0; i < n; i++)
      {
        collection.Remove(elements[i]);
      }
    }

    public static string GetName(this object obj)
    {
      // First check if it is a FrameworkElement
      if (obj is FrameworkElement element)
        return element.Name;

      // If not, try reflection to get the value of a Name property.
      var type = obj.GetType();
      try
      {
        return (string)type.GetProperty("Name").GetValue(obj, null);
      }
      catch { }

      // Last of all, try reflection to get the value of a Name field.
      try
      {
        return (string)type.GetField("Name").GetValue(obj);
      }
      catch { }

      return null;
    }
  }
}
