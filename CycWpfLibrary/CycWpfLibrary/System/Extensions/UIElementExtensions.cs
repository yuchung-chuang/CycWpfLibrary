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
  }
}
