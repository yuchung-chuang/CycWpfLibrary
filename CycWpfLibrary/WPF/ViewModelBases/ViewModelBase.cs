using System;
using System.Reflection;
using System.Windows;

namespace CycWpfLibrary
{
  /// <summary>
  /// 提供ViewMoel的基底功能。
  /// </summary>
  public class ViewModelBase : ObservableObject, IViewValidation
  {
    /// <summary>
    /// Dynamic expression
    /// </summary>
    public object this[string propertyName]
    {
      get
      {
        PropertyInfo property = GetType().GetProperty(propertyName);
        return property.GetValue(this, null);
      }
      set
      {
        PropertyInfo property = GetType().GetProperty(propertyName);
        property.SetValue(this, value, null);
      }
    }

    public bool IsViewValid { get; set; }
  }
}
