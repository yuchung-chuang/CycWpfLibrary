using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
    public bool IsValid
    {
      get
      { // return true if any input is valid
        foreach (var input in this)
        {
          if (input.IsValid)
            return true;
        }
        return false;
      }
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
  public class CycInput
  {
    public MouseButton? MouseButton { get; set; }
    public Key Key { get; set; }
    public CycKeyCollection InputKeys { get; set; }

    public bool IsEmpty => MouseButton == null && Key == Key.None && InputKeys == null ?
          true : false;
    public bool IsValid
    {
      get
      {
        // check if there is required mouse button and if the button is pressed
        // check if there is required multiple keys and each key is pressed
        // check if the required key is pressed
        return (MouseButton != null && !((MouseButton)MouseButton).IsPressed()) ||
          (InputKeys != null && !InputKeys.IsValid) ||
          (!Key.IsEmpty() && !Key.IsPressed()) ? false : true;
      }
    }
  }

  /// <summary>
  /// "mouse" and "key" as indicator
  /// ':' and ';' as starter and finisher
  /// ',' as seperator
  /// </summary>
  /// <example>
  /// mouse: left; key: leftCtrl, c
  /// </example>
  public class CycInputTypeConverter : TypeConverter
  {
    public static readonly string mouseStr = "mouse";
    public static readonly string keyStr = "key";

    private string GetSubStr(string str, int endIndex)
    {
      var start = str.IndexOf(':', endIndex);
      var end = str.IndexOf(';', endIndex);
      if (start < 0) // if no starter
        start = endIndex + 1;
      if (end < 0) // if no finisher
        end = str.Length;
      start++; //get rid of ':'
      return str.Substring(start, end - start);
    }
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      var input = new CycInput();
      var str = value.ToString().ToLower();
      var mouseStart = str.IndexOf(mouseStr);
      var keyStart = str.IndexOf(keyStr);
      if (mouseStart >= 0)
      {
        var mouseEnd = mouseStart + mouseStr.Length;
        var mouseSubStr = GetSubStr(str, mouseEnd);

        var mbAll = EnumHelpers.GetAll<MouseButton>();
        try
        {
          input.MouseButton = mbAll.First(mb => mouseSubStr.Contains(mb.ToString().ToLower()));
        }
        catch (InvalidOperationException) { } //no matched mouse button
      }

      if (keyStart >= 0) // if key specified
      {
        var keyEnd = keyStart + keyStr.Length;
        var keySubStr = GetSubStr(str, keyEnd);
        input.InputKeys = new CycKeyCollection();
        var keyAll = EnumHelpers.GetAll<Key>();
        var keySubStrs = keySubStr.Split(',');
        foreach (var subStr in keySubStrs)
        {
          var trimStr = subStr.Trim();
          var keys = keyAll.Where(key => trimStr == key.ToString().ToLower());
          foreach (var key in keys)
          {
            input.InputKeys.Add(new CycKey(key));
          }
        }
      }

      return input;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      throw new NotSupportedException();
    }
  }

  public static class CycInputHelpers
  {

  }
}
