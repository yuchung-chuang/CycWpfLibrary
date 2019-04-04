using System;
using System.Reflection;
using System.Windows;

namespace CycWpfLibrary
{
  /// <summary>
  /// 提供ViewMoel的基底功能。
  /// </summary>
  public class ViewModelBase : ObservableObject, IViewValidation, ICleanup
  {
    private IMessenger _messengerInstance;
    protected IMessenger MessengerInstance
    {
      get => _messengerInstance ?? Messenger.Default;
      set => _messengerInstance = value;
    }

    public ViewModelBase()
    {

    }

    public ViewModelBase(IMessenger messenger) : this()
    {
      MessengerInstance = messenger;
    }

    /// <summary>
    /// Unregisters this instance from the Messenger class.
    /// <para>To cleanup additional resources, override this method, clean
    /// up and then call base.Cleanup().</para>
    /// </summary>
    public virtual void Cleanup()
    {
      MessengerInstance.Unregister(this);
    }

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
