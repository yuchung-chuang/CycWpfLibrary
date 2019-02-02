using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CycWpfLibrary.Input
{
  public static class TextBoxBehaviors
  {
    public static void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      (sender as TextBox).SelectAll();
    }

    public static void TextBox_GotMouseCapture(object sender, MouseEventArgs e)
    {
      (sender as TextBox).SelectAll();
    }

    public static void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        Keyboard.ClearFocus();
      }
    }
  }
}
