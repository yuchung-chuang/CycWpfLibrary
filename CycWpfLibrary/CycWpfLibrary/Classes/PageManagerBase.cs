using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public virtual bool TurnNext(object param = null)
    {
      if (TurnNextEvent == null)
        return false;
      var isCanceled = TurnNextEvent.Invoke(this, null);
      if (!isCanceled)
        Index++;
      return isCanceled;
    }
    public virtual bool TurnBack(object param = null)
    {
      if (TurnBackEvent == null)
        return false;
      var isCanceled = TurnBackEvent.Invoke(this, null);
      if (!isCanceled)
        Index--;
      return isCanceled;
    }
    public virtual bool TurnTo(int index)
    {
      if (TurnToEvent == null)
        return false;
      var isCanceled = TurnToEvent.Invoke(this, index);
      if (!isCanceled)
        Index = index;
      return isCanceled;
    }
    /// <summary>
    /// 判斷<see cref="Index"/>是否小於頁面總數
    /// </summary>
    public virtual bool CanTurnNext(object param = null) => true;
    public virtual bool CanTurnBack(object param = null) => Index > 0;
  }
}
