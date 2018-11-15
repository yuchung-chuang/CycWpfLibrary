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

    private readonly Action _execute;
    private readonly Func<bool> _canExecute;
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法。</param>
    public RelayCommand(Action execute)
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
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public bool CanExecute(object parameter = null) => _canExecute is null || _canExecute();

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public void Execute(object parameter = null) => _execute();
  }

  /// <summary>
  /// 將方法封裝成命令的泛型類別。
  /// </summary>
  /// <typeparam name="ParameterType"><see cref="Execute(object)"/>, <see cref="CanExecute(object)"/>的參數類型。</typeparam>
  public class RelayCommand<ParameterType> : ICommand
  {
    /// <summary>
    /// 當<see cref="CanExecute(object)"/>改變時，觸發向此事件註冊過的方法
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    private Predicate<ParameterType> _canExecute;
    private Action<ParameterType> _execute;


    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<ParameterType> execute)
    {
      this._execute = execute;
      _canExecute = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<ParameterType> execute, Predicate<ParameterType> canExecute) : this(execute)
    {
      this._canExecute = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public bool CanExecute(object parameter) => _canExecute is null || _canExecute((ParameterType)parameter);

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public void Execute(object parameter) => _execute((ParameterType)parameter);
  }
}
