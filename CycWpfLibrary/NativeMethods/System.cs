using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CycWpfLibrary.NativeMethods
{
  public static class System
  {
    /// <summary>
    /// 對<paramref name="action"/>計時。
    /// </summary>
    /// <param name="action">要計時的的匿名方法。</param>
    /// <example>
    /// <code>
    /// TimeIt( () => { string s = "Your Codes"; } );
    /// </code>
    /// </example>
    public static void TimeIt(this Action action)
    {
      Stopwatch sw = new Stopwatch();//引用stopwatch物件
      sw.Restart();
      //-----目標程式-----//
      action.Invoke();
      //-----目標程式-----//
      sw.Stop();//碼錶停止
      string result = sw.Elapsed.TotalMilliseconds.ToString();
      Console.WriteLine(result);
    }

  }
}
