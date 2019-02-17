using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CycWpfLibrary
{
  public class CycInputCollection : Collection<CycInput>
  {
    public bool IsEmpty => Count == 0;
    public bool IsValid(MouseButtonEventArgs e)
    {
      // return true if any input is valid
      foreach (var input in this)
      {
        if (input.IsValid(e))
          return true;
      }
      return false;
    }
  }
  public class CycKeyCollection : Collection<CycKey>
  {
    public bool IsEmpty => Count == 0;
    public bool IsValid
    {
      get
      { // return false if any inputKey is not pressed
        foreach (var inputKey in this)
        {
          if (!inputKey.Value.IsPressed())
            return false;
        }
        return true;
      }
    }
  }
  public class CycKey
  {
    public CycKey(Key key)
    {
      Value = key;
    }
    public Key Value { get; set; }
  }

  [TypeConverter(typeof(CycInputTypeConverter))]
  public class CycInput
  {
    public int? ClickCount { get; set; }
    public MouseButton? MouseButton { get; set; }
    public Key Key { get; set; }
    public CycKeyCollection InputKeys { get; set; }

    public bool IsEmpty => MouseButton == null && Key == Key.None && InputKeys == null ?
          true : false;
    public bool IsValid(MouseButtonEventArgs e)
    {
      // check if there is required mouse button and if the button is pressed
      // check if there is required multiple keys and each key is pressed
      // check if the required key is pressed
      return (MouseButton != null && !((MouseButton)MouseButton).IsPressed()) ||
        (ClickCount != null && e != null && e.ClickCount != ClickCount) ||
        (InputKeys != null && !InputKeys.IsValid) ||
        (!Key.IsEmpty() && !Key.IsPressed()) ? false : true;
    }
  }  
}
