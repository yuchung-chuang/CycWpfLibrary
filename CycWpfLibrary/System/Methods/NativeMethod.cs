using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CycWpfLibrary
{
  public static class NativeMethod
  {
    public static Window GetWindow() => Application.Current.MainWindow;

    /// <summary>
    /// 非同步執行<paramref name="action"/>，使用<see cref="Cursors.Wait"/>。
    /// </summary>
    public static async Task CursorWaitForAsync(Action action)
    {
      await CursorWaitForAsync(action, CancellationToken.None);
    }
    
    /// <summary>
    /// 非同步執行<paramref name="action"/>，使用<see cref="Cursors.Wait"/>。
    /// </summary>
    /// <param name="token">提供工作取消的功能。</param>
    public static async Task CursorWaitForAsync(Action action, CancellationToken token)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      try //in case there is any error in the action
      {
        var task = Task.Run(action, token);
        // "await Task.Run" cannot invoke "finally", don't know why ...
        await task.ContinueWith(t => { }, token);
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    /// <summary>
    /// 非同步等待，直到<paramref name="predicate"/>條件成立或等候時間超過<paramref name="msTimeout"/>
    /// </summary>
    /// <param name="predicate">解除等待之條件</param>
    /// <param name="param">傳入<paramref name="predicate"/>之參數</param>
    /// <remarks>
    /// <see cref="SpinWait.SpinUntil(Func{bool})"/>為同步等待，在等待期間會阻擋UI執行緒導致畫面凍結
    /// </remarks>
    public static async Task WaitAsync(Predicate<object> predicate, object param, double? msTimeout = null)
    {
      var fps = 24;
      var timestep = 1000 / fps;
      var time = 0;
      while (!predicate.Invoke(param))
      {
        if (msTimeout != null && time > msTimeout)
          break;
        await Task.Delay(timestep);
        time += timestep;
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
      var sw = new Stopwatch();//引用stopwatch物件
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

    /// <summary>
    /// Make default instance for <typeparamref name="TClass"/>
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public static TClass MakeInstance<TClass>()
    {
      var type = typeof(TClass);

      var constructors = type.GetConstructors();
      // Default: get the first public constructor
      if (constructors.Length == 0)
        throw new InvalidOperationException($"{type} doesn't have public constructor.");

      // Default: get constructor without parameters
      var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);
      if (constructor == null)
        throw new InvalidOperationException($"{type} doesn't have public constructor without parameters.");
      else
        return (TClass)constructor.Invoke(null);

    }
  }
}
