using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供實現了<see cref="INotifyPropertyChanged"/>介面以及ViewModel通用功能的基底類別。
  /// 請將ViewModel繼承自此類別，並使用Fody.PropertyChanged以連結View與ViewModel。
  /// </summary>
  /// <typeparam name="ViewModelType">要繼承的ViewModel類型</typeparam>
  public class ViewModelBase<ViewModelType> : INotifyPropertyChanged where ViewModelType : class, new()
  {
    /// <summary>
    /// ViewModel的靜態實例，提供Xaml引用。
    /// </summary>
    public static ViewModelType Instance { get; private set; } = new ViewModelType();
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
