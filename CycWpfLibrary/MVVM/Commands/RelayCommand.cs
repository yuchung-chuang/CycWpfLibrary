using System;
using System.Windows.Input;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 將<see cref="Action"/>封裝成命令的類別。
  /// </summary>
  public class RelayCommand : RelayCommandBase
  {
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
    public override bool CanExecute(object parameter = null) => _canExecute is null || _canExecute();

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public override void Execute(object parameter = null) => _execute();
  }

  /// <summary>
  /// 將帶有參數的方法封裝成命令的類別。
  /// </summary>
  /// <typeparam name="TParam">參數的類型。</typeparam>
  public class RelayCommand<TParam> : RelayCommandBase
  {
    private readonly Predicate<TParam> _canExecute;
    private readonly Action<TParam> _execute;

    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<TParam> execute)
    {
      this._execute = execute;
      _canExecute = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<TParam> execute, Predicate<TParam> canExecute) : this(execute)
    {
      this._canExecute = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public override bool CanExecute(object parameter) => _canExecute is null || _canExecute((TParam)parameter);

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public override void Execute(object parameter) => _execute((TParam)parameter);
  }

  /// <summary>
  /// 將帶有參數及回傳值的方法封裝成命令的類別
  /// </summary>
  /// <typeparam name="TParam">參數的類型。</typeparam>
  /// <typeparam name="TResult">回傳值的類型。</typeparam>
  public class RelayCommand<TParam, TResult> : RelayCommandBase
  {
    private Predicate<TParam> _canExecute;
    private Func<TParam, TResult> _execute;

    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="TParam"/>類型的參數以及<typeparamref name="TResult"/>類型的回傳值。</param>
    public RelayCommand(Func<TParam, TResult> execute)
    {
      this._execute = execute;
      _canExecute = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="TParam"/>類型的參數以及<typeparamref name="TResult"/>類型的回傳值。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="TParam"/>類別的參數。</param>
    public RelayCommand(Func<TParam, TResult> execute, Predicate<TParam> canExecute) : this(execute)
    {
      this._canExecute = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public override bool CanExecute(object parameter) => _canExecute is null || _canExecute((TParam)parameter);

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public override void Execute(object parameter) => _execute((TParam)parameter);
  }
}
