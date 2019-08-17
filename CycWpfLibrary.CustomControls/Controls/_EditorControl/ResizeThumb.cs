﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using static System.Math;

namespace CycWpfLibrary.CustomControls
{
  public class ResizeThumb : Thumb
  {
    public ResizeThumb()
    {
      DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
    }

    private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
      var designerItem = this.DataContext as Control;

      if (designerItem != null)
      {
        double deltaVertical, deltaHorizontal;

        switch (VerticalAlignment)
        {
          case VerticalAlignment.Bottom:
            deltaVertical = Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
            designerItem.Height -= deltaVertical;
            break;
          case VerticalAlignment.Top:
            deltaVertical = Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
            Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaVertical);
            designerItem.Height -= deltaVertical;
            break;
          default:
            break;
        }

        switch (HorizontalAlignment)
        {
          case HorizontalAlignment.Left:
            deltaHorizontal = Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
            Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
            designerItem.Width -= deltaHorizontal;
            break;
          case HorizontalAlignment.Right:
            deltaHorizontal = Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
            designerItem.Width -= deltaHorizontal;
            break;
          default:
            break;
        }
      }

      e.Handled = true;
    }
  }
}
