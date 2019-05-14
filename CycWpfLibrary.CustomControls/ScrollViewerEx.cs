using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CycWpfLibrary.MVVM;

namespace CycWpfLibrary.CustomControls
{
  public class ScrollViewerEx : ScrollViewer
  {
    public ScrollViewerEx()
    {
      RegisterMouseTilt();
    }

    #region MouseTilt
    private void RegisterMouseTilt()
    {
      Loaded += (scrollviewerSender, loadArgs) =>
      {
        var window = Window.GetWindow(this) as CustomWindowBase;
        window.MouseHWheel += (windowSender, HWheelArgs) =>
        {
          OnMouseHWheel(HWheelArgs.Tilt); // propagate event
        };
      };
    }

    public event EventHandler<MouseTiltEventArgs> MouseHWheel;
    protected virtual void OnMouseHWheel(int delta)
    {
      MouseHWheel?.Invoke(this, new MouseTiltEventArgs(delta));
      ScrollToHorizontalOffset(HorizontalOffset + delta * 0.5);
    }
    #endregion
  }
}
