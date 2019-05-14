using System;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 將帶有參數的方法封裝成命令的類別。
  /// </summary>
  /// <typeparam name="TParam">參數的類型。</typeparam>
  public class RelayCommand<TParam> : RelayCommandBase
  {
    public readonly Predicate<TParam> canExecute;
    public readonly Action<TParam> execute;

    /// <summary>
    /// 初始化<see cref="RelayCommand"/>物件
    /// </summary>
    public RelayCommand()
    {

    }

    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並使此命令永遠可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<TParam> execute) : this()
    {
      this.execute = execute;
      canExecute = (obj) => true;
    }
    /// <summary>
    /// 將方法<paramref name="execute"/>封裝成命令，並由<paramref name="canExecute"/>判斷此命令是否可執行。
    /// </summary>
    /// <param name="execute">要封裝成命令的方法，需有一個<see cref="object"/>類別的參數。</param>
    /// <param name="canExecute">判斷<paramref name="execute"/>是否可以執行的方法，需有一個<see cref="object"/>類別的參數。</param>
    public RelayCommand(Action<TParam> execute, Predicate<TParam> canExecute) : this(execute)
    {
      this.canExecute = canExecute;
    }

    /// <summary>
    /// 判斷<see cref="Execute(object)"/>是否可以執行。
    /// </summary>
    public override bool CanExecute(object parameter) => canExecute is null || canExecute((TParam)parameter);

    /// <summary>
    /// 執行封裝的方法。
    /// </summary>
    public override void Execute(object parameter) => execute((TParam)parameter);
  }
}
