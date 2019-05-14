using System.ComponentModel;
using System.Windows;

namespace CycWpfLibrary.MVVM
{
  public class ObservableDependencyObject : DependencyObject, INotifyPropertyChanged
  {
    /// <summary>
    /// 當其他屬性變更時觸發此事件。
    /// 請安裝Fody.PropertyChanged套件以自動呼叫此事件。
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// 提供方法呼叫<see cref="PropertyChanged"/>事件。
    /// </summary>
    /// <param name="property">發生<see cref="PropertyChanged"/>的屬性。</param>
    public void OnPropertyChanged(string property)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
  }
}
