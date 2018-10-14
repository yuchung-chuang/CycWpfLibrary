using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 相依屬性的基底類別。
  /// </summary>
  /// <typeparam name="Parent">相依屬性依附的父類別。</typeparam>
  /// <typeparam name="Property">相依屬性的類別。</typeparam>
  public abstract class AttachedPropertyBase<Parent, Property> 
    where Parent : AttachedPropertyBase<Parent, Property>, new()
  {
    /// <summary>
    /// 父類別的單例實例。
    /// </summary>
    public static Parent Instance { get; private set; } = new Parent();

    /// <summary>
    /// 宣告相依屬性，並綁定屬性改變的回呼。
    /// </summary>
    public static DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(Property), typeof(AttachedPropertyBase<Parent, Property>), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

    /// <summary>
    /// 當屬性改變時，系統自動觸發的回呼。
    /// </summary>
    /// <param name="sender">發生屬性改變的物件。</param>
    /// <param name="e">屬性改變的參數。</param>
    private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      //觸發屬性改變事件。
      Instance.ValueChanged?.Invoke(sender, e);

      //觸發父類別屬性改變的覆寫方法。
      Instance.OnValueChanged(sender, e);
    }

    #region events and virtual methods
    /// <summary>
    /// 當屬性改變時，提供給外界註冊、觸發的事件。
    /// </summary>
    public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged;

    /// <summary>
    /// 當屬性改變時，提供給外界覆寫的方法。
    /// </summary>
    /// <param name="sender">發生屬性改變的物件。</param>
    /// <param name="e">屬性改變的參數。</param>
    public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {

    }
    #endregion

    #region public methods
    /// <summary>
    /// 取得相依屬性的值。
    /// </summary>
    /// <param name="sender">擁有相依屬性的物件。</param>
    public static Property GetValue(DependencyObject sender) => (Property)sender.GetValue(ValueProperty);

    /// <summary>
    /// 設定相依屬性的值。
    /// </summary>
    /// <param name="sender">擁有相依屬性的物件。</param>
    /// <param name="value">要設定給相依屬性的值。</param>
    public static void SetValue(DependencyObject sender, Property value) => sender.SetValue(ValueProperty, value);
    #endregion
  }
}
