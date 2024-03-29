﻿using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CycWpfLibrary.CustomControls
{
  public class MoveThumb : Thumb
  {
    public MoveThumb()
    {
      DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
    }

    private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
      var designerItem = this.DataContext as Control;

      if (designerItem != null)
      {
        var left = Canvas.GetLeft(designerItem);
        var top = Canvas.GetTop(designerItem);

        Canvas.SetLeft(designerItem, left + e.HorizontalChange);
        Canvas.SetTop(designerItem, top + e.VerticalChange);
      }
    }
  }
}
