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
  public delegate void TurnEventHandler(object sender, TurnEventArgs e);
  public class TurnEventArgs : EventArgs
  {
    public bool IsCancel { get; set; }
  }
  public delegate void TurnToEventHandler(object sender, TurnToEventArgs e);
  public class TurnToEventArgs : TurnEventArgs
  {
    public int Index { get; set; }
  }

  public abstract class PageManagerBase : ObservableObject
  {
    public virtual int Index { get; set; }

    /// <summary>
    /// 根據<see cref="Index"/>來取得當前頁面。
    /// </summary>
    public abstract FrameworkElement CurrentPage { get; }

    public ICommand TurnNextCommand { get; set; }
    public ICommand TurnBackCommand { get; set; }

    public event TurnEventHandler TurnNextEvent;
    public event TurnEventHandler TurnBackEvent;
    public event TurnToEventHandler TurnToEvent;

    public static bool IsSuccess(bool? turnResult) => turnResult == null || turnResult == true;
    /// <summary>
    /// Turn to next page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnNext(object param = null)
    {
      var args = new TurnEventArgs();
      TurnNextEvent?.Invoke(this, args);
      if (!args.IsCancel)
        Index++;
      return !args.IsCancel;
    }
    /// <summary>
    /// Turn to previous page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnBack(object param = null)
    {
      var args = new TurnEventArgs();
      TurnBackEvent?.Invoke(this, args);
      if (!args.IsCancel)
        Index--;
      return !args.IsCancel;
    }
    /// <summary>
    /// Turn to <paramref name="index"/> page. If this action is successful, return true. Otherwise, return false.
    /// </summary>
    public virtual bool TurnTo(int index)
    {
      var args = new TurnToEventArgs { Index = index };
      TurnToEvent?.Invoke(this, args);
      if (!args.IsCancel)
        Index = index;
      return !args.IsCancel;
    }
    /// <summary>
    /// 判斷<see cref="Index"/>是否小於頁面總數
    /// </summary>
    public virtual bool CanTurnNext(object param = null) => true;
    public virtual bool CanTurnBack(object param = null) => Index > 0;
  }
}
