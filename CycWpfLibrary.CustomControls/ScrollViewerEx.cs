using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CycWpfLibrary.CustomControls
{
  public class ScrollViewerEx : ScrollViewer
  {

    public ScrollViewerEx()
    {
      Loaded += (scrollviewerSender, loadArgs) =>
      {
        var window = Window.GetWindow(this) as CustomWindowBase;
        window.MouseTilt += (windowSender, tiltArgs) =>
        {
          OnMouseTilt(tiltArgs.Tilt); // propagate event
        };
      };
    }

    public event EventHandler<MouseTiltEventArgs> MouseTilt;
    protected virtual void OnMouseTilt(int tilt)
    {
      MouseTilt?.Invoke(this, new MouseTiltEventArgs(tilt));
      OnMouseTilting(tilt);
    }

    /// <summary>
    /// Processing horizontal scrolling.
    /// </summary>
    /// <param name="tilt">The amount of tilt.</param>
    protected virtual void OnMouseTilting(int tilt)
    {
      ScrollToHorizontalOffset(HorizontalOffset + tilt);
    }
  }
}
