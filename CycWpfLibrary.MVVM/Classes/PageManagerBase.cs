using System;
using System.Windows;
using System.Windows.Input;

namespace CycWpfLibrary.MVVM
{
  public abstract class PageManagerBase : ObservableObject
  {
    public virtual int Index { get; set; }

    /// <summary>
    /// 根據<see cref="Index"/>來取得當前頁面。
    /// </summary>
    public abstract FrameworkElement CurrentPage { get; }

    public ICommand TurnNextCommand { get; set; }
    public ICommand TurnBackCommand { get; set; }

    public event EventHandler TurnNextEvent;
    public event EventHandler TurnBackEvent;
    public event EventHandler TurnToEvent;

    public virtual bool TurnNextValidation() => true;
    public virtual bool TurnBackValidation() => true;
    public virtual bool TurnToValidation(int index) => true;
    /// <summary>
    /// Turn to next page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnNext(object param = null)
    {
      var result = TurnNextValidation();
      if (result)
      {
        TurnNextEvent?.Invoke(this, new EventArgs());
        Index++;
      }
      return result;
    }
    /// <summary>
    /// Turn to previous page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnBack(object param = null)
    {
      var result = TurnBackValidation();
      if (result)
      {
        TurnBackEvent?.Invoke(this, new EventArgs());
        Index--;
      }
      return result;
    }
    /// <summary>
    /// Turn to <paramref name="index"/> page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnTo(int index)
    {
      var turns = index - Index;
      if (turns > 0)
      {
        for (var i = 0; i < turns; i++)
          if (!TurnNext())
            return false;
      }
      else if (turns < 0)
      {
        for (var i = 0; i > turns; i--)
          if (!TurnBack())
            return false;
      }
      TurnToEvent?.Invoke(this, new EventArgs());
      return true;
    }

    /// <summary>
    /// 判斷<see cref="Index"/>是否小於頁面總數
    /// </summary>
    public virtual bool CanTurnNext(object param = null) => true;
    public virtual bool CanTurnBack(object param = null) => Index > 0;
  }
}
