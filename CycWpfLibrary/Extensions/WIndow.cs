using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Application = System.Windows.Application;
using Panel = System.Windows.Controls.Panel;
using PointWpf = System.Windows.Point;
using SP = System.Windows.SystemParameters;

namespace CycWpfLibrary
{
  public enum TaskBarDock
  {
    None = 0,
    Left,
    Right,
    Top,
    Bottom,
  }
  public static class WindowExtensions
  {
    /// <summary>
    /// 取得作業系統的Dpi放大率，需要在<paramref name="window"/>已經載入後使用。
    /// </summary>
    public static PointWpf GetDpiRatio(this Window window)
    {
      var DpiRatio = new PointWpf(1, 1);
      var source = PresentationSource.FromVisual(window);
      if (source == null)
      {
        throw new NullReferenceException("找不到此Window的Visual");
      }
      else
      {
        DpiRatio.X = source.CompositionTarget.TransformToDevice.M11;
        DpiRatio.Y = source.CompositionTarget.TransformToDevice.M22;
      }
      return DpiRatio;
    }

    /// <summary>
    /// 取得當下滑鼠在螢幕上的座標。
    /// </summary>
    public static PointWpf GetMouseScreenPos(this Window window) => window.PointToScreen(Mouse.GetPosition(window));

    /// <summary>
    /// 將<paramref name="visual"/>座標系中的點<paramref name="point"/>轉換成考慮過DPI螢幕坐標系的點座標。
    /// </summary>
    public static PointWpf PointToScreenDPI(this Visual visual, PointWpf point) => visual.PointToScreen(point).Divide(Application.Current.MainWindow.GetDpiRatio());

    /// <summary>
    /// 確保<paramref name="window"/>中的Content是<see cref="Panel"/>，若否，則將Content替換成<see cref="Grid"/>，並將原先的Content放入<see cref="Grid"/>中。
    /// </summary>
    public static void EnsurePanelContent(this Window window)
    {
      if (!(window.Content is Panel))
      {
        var grid = new Grid();
        grid.Children.Add(window.Content as UIElement);
        window.Content = grid;
      }
    }

    /// <summary>
    /// 若<see cref="Window"/>超出螢幕邊界，將其移回邊界內
    /// </summary>
    public static void ShiftWindowOntoScreen(this Window window)
    {
      if (window.Top < SP.VirtualScreenTop)
        window.Top = SP.VirtualScreenTop;

      if (window.Left < SP.VirtualScreenLeft)
        window.Left = SP.VirtualScreenLeft;

      if (window.Left + window.ActualWidth > SP.VirtualScreenLeft + SP.VirtualScreenWidth)
        window.Left = SP.VirtualScreenWidth + SP.VirtualScreenLeft - window.ActualWidth;

      if (window.Top + window.ActualHeight > SP.VirtualScreenTop + SP.VirtualScreenHeight)
        window.Top = SP.VirtualScreenHeight + SP.VirtualScreenTop - window.ActualHeight;

      // Shift window away from taskbar.
      var taskbars = ScreenMethods.GetTaskBarLocationPerScreen();
      var windowRect = new Rect(window.Left, window.Top, window.ActualWidth, window.ActualHeight);
      foreach (var taskbar in taskbars)
      {
        if (windowRect.IntersectsWith(taskbar.Rect))
        {
          switch (taskbar.Dock)
          {
            default:
            case TaskBarDock.None:
              break;
            case TaskBarDock.Left:
              window.Left += taskbar.Width;
              break;
            case TaskBarDock.Right:
              window.Left -= taskbar.Width;
              break;
            case TaskBarDock.Top:
              window.Top += taskbar.Width;
              break;
            case TaskBarDock.Bottom:
              window.Top -= taskbar.Width;
              break;
          }
        }
      }

    }
  }
}
