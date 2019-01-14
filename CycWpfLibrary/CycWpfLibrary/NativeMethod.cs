using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CycWpfLibrary.Media;

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
      await Task.Run(action);
      Application.Current.MainWindow.Cursor = Cursors.Arrow;
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

    /// <summary>
    /// 取得滑鼠相對於<paramref name="element"/>在螢幕上的座標，此結果不會受到<see cref="UIElement.RenderTransform"/>的影響。
    /// </summary>
    public static Point GetAbsolutePosition(this MouseEventArgs e, UIElement element)
    {
      var transformsTemplate = (element.RenderTransform as TransformGroup).Children;
      var transformsIdentity = new TransformCollection()
      {
        new TranslateTransform(),
        new ScaleTransform(),
        new RotateTransform(),
        new SkewTransform(),
      };
      // 重設UIElement的transforms
      (element.RenderTransform as TransformGroup).Children = transformsIdentity;
      // 取得座標
      var absolute = e.GetPosition(element);
      // 復原transforms
      (element.RenderTransform as TransformGroup).Children = transformsTemplate;
      return absolute;
    }

    /// <summary>
    /// 交換泛型數據。
    /// </summary>
    /// <remarks>由於傳入指標，當數據為參考型別時，不會更動擁有同樣數據的其他變數值。</remarks>
    public static void Swap<T>(ref T x, ref T y) => (x, y) = (y, x);
  }
}
