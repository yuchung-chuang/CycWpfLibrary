using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供實現了<see cref="INotifyPropertyChanged"/>介面以及ViewModel通用功能的基底類別。
  /// 請將ViewModel繼承自此類別，並使用Fody.PropertyChanged以連結View與ViewModel。
  /// </summary>
  /// <typeparam name="ViewModelType">要繼承的ViewModel類型</typeparam>
  public class ViewModelBase : NotifyableObject
  {

  }
}
