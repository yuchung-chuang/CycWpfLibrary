using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary.Input
{
  public static class InputExtensions
  {
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
      var transformsIdentity = new TransformCollection();
      // 重設UIElement的transforms
      (element.RenderTransform as TransformGroup).Children = transformsIdentity;
      // 取得座標
      var absolute = e.GetPosition(element);
      // 復原transforms
      (element.RenderTransform as TransformGroup).Children = transformsTemplate;
      return absolute;
    }

    public static bool IsMouseButtonPressed(this MouseEventArgs e, MouseButton mouseButton)
    {
      switch (mouseButton)
      {
        case MouseButton.Left:
          return e.LeftButton == MouseButtonState.Pressed;
        case MouseButton.Middle:
          return e.MiddleButton == MouseButtonState.Pressed;
        case MouseButton.Right:
          return e.RightButton == MouseButtonState.Pressed;
        case MouseButton.XButton1:
          return e.XButton1 == MouseButtonState.Pressed;
        case MouseButton.XButton2:
          return e.XButton2 == MouseButtonState.Pressed;
        default:
          return false;
      }
    }
  }
}
