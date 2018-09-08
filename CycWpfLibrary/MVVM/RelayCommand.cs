using System;
using System.Windows.Input;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 將方法封裝成命令的類別。
  /// </summary>
  public class RelayCommand : ICommand
  {
    /// <summary>
    /// 當<see cref="CanExecute(object)"/>改變時，觸發向此事件註冊過的方法
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    Action<object> _execute;
    Predicate<object> _canExecute;
    /// <summary>
    /// 初始化<see cref="RelayCommand"/>的執行個體。
    /// </summary>
    public RelayCommand()
    {

    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<object> execute) : this()
    {
      _execute = execute;
      _canExecute = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute) : this(execute)
    {
      _canExecute = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    /// <param name="parameter">需要輸入的參數，若不需要請輸入<see cref="null"/>。</param>
    public bool CanExecute(object parameter)
    {
      return _canExecute == null || _canExecute(parameter); ;
    }

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    /// <param name="parameter">需要輸入的參數，若不需要請輸入<see cref="null"/>。</param>
    public void Execute(object parameter)
    {
      _execute(parameter);
    }
  }
}
