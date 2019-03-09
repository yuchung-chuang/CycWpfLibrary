using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using static System.Math;
using PixelFormatWinForm = System.Drawing.Imaging.PixelFormat;
using PointWF = System.Drawing.Point;
using Screen = System.Windows.Forms.Screen;

namespace CycWpfLibrary
{
  public static class ScreenMethods
  {
    /// <summary>
    /// 取得系統工具列在螢幕中的位置
    /// </summary>
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

    /// <summary>
    /// 將螢幕內<paramref name="rect"/>中的像素拷貝成<see cref="BitmapSource"/>。
    /// </summary>
    public static Bitmap CopyScreen(Rect rect)
    {
      using (var screenBmp = new Bitmap(
          (int)rect.Width,
          (int)rect.Height,
          PixelFormatWinForm.Format32bppArgb))
      {
        using (var bmpGraphics = Graphics.FromImage(screenBmp))
        {
          var rectWF = rect.ToWinForm();
          bmpGraphics.CopyFromScreen(rectWF.Left, rectWF.Top, 0, 0, rectWF.Size);
          return screenBmp.Clone() as Bitmap;
        }
      }
    }

    /// <summary>
    /// 直接取得滑鼠在螢幕中的位置，不須透過<see cref="Window"/>
    /// </summary>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref PointWF lpPoint);
  }
}
