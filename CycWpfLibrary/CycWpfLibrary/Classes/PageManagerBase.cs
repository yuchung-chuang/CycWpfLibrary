using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CycWpfLibrary
{

  public abstract class PageManagerBase : ObservableObject
  {
    public virtual int Index { get; set; }

    /// <summary>
    /// 根據<see cref="Index"/>來取得當前頁面。
    /// </summary>
    public abstract UserControl CurrentPage { get; }

    public ICommand TurnNextCommand { get; set; }
    public ICommand TurnBackCommand { get; set; }

    public event Func<object, EventArgs, bool> TurnNextEvent;
    public event Func<object, EventArgs, bool> TurnBackEvent;
    public event Func<object, int, bool> TurnToEvent;

    public static bool IsSuccess(bool? turnResult) => turnResult == null || turnResult == true;
    /// <summary>
    /// Turn to next page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnNext(object param = null)
    {
      bool turnResult = IsSuccess(TurnNextEvent?.Invoke(this, null));
      if (turnResult)
        Index++;
      return turnResult;
    }
    /// <summary>
    /// Turn to previous page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnBack(object param = null)
    {
      bool turnResult = IsSuccess(TurnBackEvent?.Invoke(this, null));
      if (turnResult)
        Index--;
      return turnResult;
    }
    /// <summary>
    /// Turn to <paramref name="index"/> page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnTo(int index)
    {
      bool turnResult = IsSuccess( TurnToEvent?.Invoke(this, index));
      if (turnResult)
        Index = index;
      return turnResult;
    }
    /// <summary>
    /// 判斷<see cref="Index"/>是否小於頁面總數
    /// </summary>
    public virtual bool CanTurnNext(object param = null) => true;
    public virtual bool CanTurnBack(object param = null) => Index > 0;
  }
}
