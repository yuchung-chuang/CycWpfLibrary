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
    public bool IsViewValid { get; set; }
  }
}
