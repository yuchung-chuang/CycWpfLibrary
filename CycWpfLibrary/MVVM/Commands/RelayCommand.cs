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

    private Action _execute;
    private Func<bool> _canExecute;
    private Action<object> _executeInput;
    private Predicate<object> _canExecuteInput;
    /// <summary>
    /// 初始化<see cref="RelayCommand"/>的執行個體。
    /// </summary>
    public RelayCommand()
    {

    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法。</param>
    public RelayCommand(Action execute) : this()
    {
      _execute = execute;
      _canExecute = () => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法。</param>
    /// <param name="canExecute">判斷<paramref name="action"/>是否可執行的方法。</param>
    public RelayCommand(Action execute, Func<bool> canExecute) : this(execute)
    {
      _canExecute = canExecute;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<object> execute) : this()
    {
      _executeInput = execute;
      _canExecuteInput = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute) : this(execute)
    {
      _canExecuteInput = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public bool CanExecute(object parameter = null)
    {
      return (parameter is null) ? 
        _canExecute is null || _canExecute() : 
        _canExecuteInput is null || _canExecuteInput(parameter); 
    }

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public void Execute(object parameter = null)
    {
      if (parameter is null)
      {
        _execute();
      }
      else
      {
        _executeInput(parameter);
      }
    }
  }
}
