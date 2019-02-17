using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycWpfLibrary
{
  /// <summary>
  /// 將方法封裝成命令的基底類別。
  /// </summary>
  public abstract class RelayCommandBase : ICommand
  {
    /// <summary>
    /// 當<see cref="CanExecute(object)"/>改變時，自動將方法註冊給<see cref="CommandManager"/>
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public abstract bool CanExecute(object parameter);
    public abstract void Execute(object parameter);
  }
}
