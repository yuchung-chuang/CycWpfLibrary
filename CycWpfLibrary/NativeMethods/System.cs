using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

    /// <summary>
    /// 取得<paramref name="path"/>之PackUri物件
    /// </summary>
    /// <param name="path">應用程式資料夾內之路徑</param>
    public static Uri PackUri(this string path)
    {
      return new Uri($"pack://application:,,,/" + path);
    }

    /// <summary>
    /// 取得作業系統的Dpi放大率，需要在<paramref name="window"/>已經載入後使用。
    /// </summary>
    public static Point GetDpiRatio(this Window window)
    {
      Point Dpi = new Point();
      Point DpiRatio = new Point(1, 1);
      PresentationSource source = PresentationSource.FromVisual(window);
      if (source == null)
      {
        throw new NullReferenceException("找不到此Window的Visual"); 
      }
      else
      {
        Dpi.X = 96.0 * source.CompositionTarget.TransformToDevice.M11;
        Dpi.Y = 96.0 * source.CompositionTarget.TransformToDevice.M22;
        DpiRatio.X = Dpi.X / 96;
        DpiRatio.Y = Dpi.Y / 96;
      }
      return DpiRatio;
    }

    /// <summary>
    /// 取得當下滑鼠在螢幕上的座標。
    /// </summary>
    public static Point GetMousePosOnScreen(this Window window)
    {
      return window.PointToScreen(Mouse.GetPosition(window));
    }
  }
}
