using CycWpfLibrary.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary
{
  public static class NativeMethod
  {
    /// <summary>
    /// 非同步執行<paramref name="action"/>，並使用<see cref="Cursors.Wait"/>。
    /// </summary>
    public static async Task CursorWaitForAsync(Action action)
    {
      Application.Current.MainWindow.Cursor = Cursors.Wait;
      try //in case there is any error in the action
      {
        await Task.Run(action);
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        Application.Current.MainWindow.Cursor = Cursors.Arrow;
      }
    }

    /// <summary>
    /// 對<paramref name="action"/>計時。
    /// </summary>
    /// <param name="action">要計時的的匿名方法。</param>
    /// <example>
    /// <code>
    /// TimeIt( () => { string s = "Your Codes"; } );
    /// </code>
    /// </example>
    public static double TimeIt(Action action)
    {
      Stopwatch sw = new Stopwatch();//引用stopwatch物件
      sw.Restart();
      //-----目標程式-----//
      action.Invoke();
      //-----目標程式-----//
      sw.Stop();//碼錶停止
      var ms = sw.Elapsed.TotalMilliseconds;
      Debug.WriteLine(ms.ToString());
      return ms;
    }

    /// <summary>
    /// 取得<paramref name="path"/>之PackUri物件
    /// </summary>
    /// <param name="path">應用程式資料夾內之路徑</param>
    public static Uri PackUri(this string path)
    {
      return new Uri($"pack://application:,,,/" + path);
    }

    /// <summary>
    /// 交換泛型數據。
    /// </summary>
    /// <remarks>由於傳入指標，當數據為參考型別時，不會更動擁有同樣數據的其他變數值。</remarks>
    public static void Swap<T>(ref T x, ref T y) => (x, y) = (y, x);
  }
}
