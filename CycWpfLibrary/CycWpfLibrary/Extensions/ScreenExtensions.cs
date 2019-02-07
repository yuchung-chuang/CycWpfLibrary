using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static System.Math;
using Application = System.Windows.Application;
using Panel = System.Windows.Controls.Panel;
using Rectangle = System.Drawing.Rectangle;
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
  public static class ScreenExtensions
  {
    /// <summary>
    /// 取得作業系統的Dpi放大率，需要在<paramref name="window"/>已經載入後使用。
    /// </summary>
    public static Point GetDpiRatio(this Window window)
    {
      Point DpiRatio = new Point(1, 1);
      PresentationSource source = PresentationSource.FromVisual(window);
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
    public static Point GetMousePosOnScreen(this Window window) => window.PointToScreen(Mouse.GetPosition(window));

    /// <summary>
    /// 將<paramref name="visual"/>座標系中的點<paramref name="point"/>轉換成考慮過DPI螢幕坐標系的點座標。
    /// </summary>
    public static Point PointToScreenDPI(this Visual visual, Point point) => visual.PointToScreen(point).Divide(Application.Current.MainWindow.GetDpiRatio());

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
      var taskbars = GetTaskBarLocationPerScreen();
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

    public static List<(TaskBarDock Dock, double Width, Rect Rect)> GetTaskBarLocationPerScreen()
    {
      var dpiRatio = Application.Current.MainWindow.GetDpiRatio();
      List<(TaskBarDock, double, Rect)> taskBars = new List<(TaskBarDock, double, Rect)>();
      foreach (var screen in Screen.AllScreens)
      {
        if (screen.Bounds.Equals(screen.WorkingArea))
          continue; // No taskbar on this screen.

        TaskBarDock dock = TaskBarDock.None;
        double width = 0;
        Rect rect = new Rect();

        var leftDockedWidth = Abs(Abs(screen.Bounds.Left) - Abs(screen.WorkingArea.Left));
        var topDockedHeight = Abs(Abs(screen.Bounds.Top) - Abs(screen.WorkingArea.Top));
        var rightDockedWidth = screen.Bounds.Width - leftDockedWidth - screen.WorkingArea.Width;
        var bottomDockedHeight = screen.Bounds.Height - topDockedHeight - screen.WorkingArea.Height;
        if (leftDockedWidth > 0)
        {
          dock = TaskBarDock.Left;
          width = leftDockedWidth / dpiRatio.X;
          rect.X = screen.Bounds.Left;
          rect.Y = screen.Bounds.Top;
          rect.Width = leftDockedWidth;
          rect.Height = screen.Bounds.Height;
        }
        else if (rightDockedWidth > 0)
        {
          dock = TaskBarDock.Right;
          width = rightDockedWidth / dpiRatio.X;
          rect.X = screen.WorkingArea.Right;
          rect.Y = screen.Bounds.Top;
          rect.Width = rightDockedWidth;
          rect.Height = screen.Bounds.Height;
        }
        else if (topDockedHeight > 0)
        {
          dock = TaskBarDock.Top;
          width = topDockedHeight / dpiRatio.Y;
          rect.X = screen.WorkingArea.Left;
          rect.Y = screen.Bounds.Top;
          rect.Width = screen.WorkingArea.Width;
          rect.Height = topDockedHeight;
        }
        else if (bottomDockedHeight > 0)
        {
          dock = TaskBarDock.Bottom;
          width = bottomDockedHeight / dpiRatio.Y;
          rect.X = screen.WorkingArea.Left;
          rect.Y = screen.WorkingArea.Bottom;
          rect.Width = screen.WorkingArea.Width;
          rect.Height = bottomDockedHeight;
        }
        rect = rect.Divide(dpiRatio);
        taskBars.Add((dock, width, rect));
      }
      return taskBars;
    }
  }
}
