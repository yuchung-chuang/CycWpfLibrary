using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CycWpfLibrary.Controls
{
  public abstract class PageManagerBase : NotifyableObject
  {
    public virtual int Index { get; set; }
    
    /// <summary>
    /// 根據<see cref="Index"/>來取得當前頁面。
    /// </summary>
    public abstract UserControl CurrentPage { get; }

    public ICommand TurnNextCommand { get; set; }
    public ICommand TurnBackCommand { get; set; }

    public event Action TurnNextEvent;
    public event Action TurnBackEvent;

    public virtual void TurnNext()
    {
      TurnNextEvent?.Invoke();
      Index++;
    }
    public virtual void TurnBack()
    {
      TurnBackEvent?.Invoke();
      Index--;
    }
    /// <summary>
    /// 判斷<see cref="Index"/>是否小於頁面總數
    /// </summary>
    public virtual bool CanTurnNext() => true;
    public virtual bool CanTurnBack() => Index > 0;
  }
}
