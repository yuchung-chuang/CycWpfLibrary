using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供實現了<see cref="INotifyPropertyChanged"/>介面的基底類別。
  /// 請將ViewModel繼承自此類別，並使用Fody.PropertyChanged以連結View與ViewModel。
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged
  {
    /// <summary>
    /// 當其他屬性變更時觸發此事件。
    /// 請使用Fody.PropertyChanged以自動呼叫此事件。
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
  }
}
