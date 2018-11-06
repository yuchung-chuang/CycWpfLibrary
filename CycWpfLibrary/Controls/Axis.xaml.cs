﻿using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CycWpfLibrary.Math;

namespace CycWpfLibrary.Controls
{
  public enum AdjustType
  {
    None = 0x0,
    Left = 0x1,
    Top = 0x2,
    Right = 0x4,
    Bottom = 0x8,
    LeftTop = Left | Top,
    RightTop = Right | Top,
    LeftBottom = Left | Bottom,
    RightBottom = Right | Bottom,
  }
  public static class AdjustTypeExtension
  {
    public static bool Contain(this AdjustType typeA, AdjustType typeB) => (typeA & typeB) == typeB;

    public static AdjustType Add(this AdjustType typeA, AdjustType typeB) => typeA | typeB;

    public static AdjustType Remove(this AdjustType typeA, AdjustType typeB) => typeA ^ typeB;
  }

  public partial class Axis : ViewModelUserControl
  {
    public Axis()
    {
      InitializeComponent();

      gridMain.DataContext = this;
    }

    public string AxisBrush { get; set; }
    public string ShadowBrush { get; set; }

    #region AxisProperties
    public static readonly DependencyProperty AxisLeftProperty = DependencyProperty.Register(nameof(AxisLeft), typeof(double), typeof(Axis), new PropertyMetadata((d, e) =>
    {
      var sender = d as Axis;
      sender.OnPropertyChanged(nameof(AxisMargin));
    }));
    public double AxisLeft
    {
      get => (double)GetValue(AxisLeftProperty);
      set => SetValue(AxisLeftProperty, Clamp(value, AxisRight - tol, 0));
    }
    public static readonly DependencyProperty AxisTopProperty = DependencyProperty.Register(nameof(AxisTop), typeof(double), typeof(Axis), new PropertyMetadata((d, e) =>
    {
      var sender = d as Axis;
      sender.OnPropertyChanged(nameof(AxisMargin));
      sender.OnPropertyChanged(nameof(AxisRight));
      sender.OnPropertyChanged(nameof(TopBorderMargin));
      sender.OnPropertyChanged(nameof(RightBorderMargin));
    }));
    public double AxisTop
    {
      get => (double)GetValue(AxisTopProperty);
      set => SetValue(AxisTopProperty, Clamp(value, AxisBottom - tol, 0));
    }
    public static readonly DependencyProperty AxisWidthProperty = DependencyProperty.Register(nameof(AxisWidth), typeof(double), typeof(Axis), new PropertyMetadata((d, e) =>
    {
      var sender = d as Axis;
      sender.AxisRight = -1; //fire OnPropertyChanged
    }));
    public double AxisWidth
    {
      get => (double)GetValue(AxisWidthProperty);
      set => SetValue(AxisWidthProperty, Clamp(value, double.MaxValue, tol));
    }
    public static readonly DependencyProperty AxisHeightProperty = DependencyProperty.Register(nameof(AxisHeight), typeof(double), typeof(Axis), new PropertyMetadata((d, e) =>
    {
      var sender = d as Axis;
      sender.AxisBottom = -1; //fire OnPropertyChanged
    }));
    public double AxisHeight
    {
      get => (double)GetValue(AxisHeightProperty);
      set => SetValue(AxisHeightProperty, Clamp(value, double.MaxValue, tol));
    }
    public double AxisRight
    {
      get => AxisLeft + AxisWidth;
      set { }
    }
    public double AxisBottom
    {
      get => AxisTop + AxisHeight;
      set { }
    }
    #endregion

    #region Margins
    public Thickness AxisMargin => new Thickness(AxisLeft, AxisTop, 0, 0);
    public Thickness TopBorderMargin => new Thickness(0, AxisTop, 0, 0);
    public Thickness BottomBorderMargin => new Thickness(0, AxisBottom, 0, 0);
    public Thickness RightBorderMargin => new Thickness(AxisRight, AxisTop, 0, 0);
    #endregion

    #region Event Callbacks
    private bool IsAdjust = false;
    private AdjustType State = AdjustType.None;
    private AdjustType GetState(Point mousePos)
    {
      var state = new AdjustType();
      if (ApproxEqual(mousePos.X, AxisLeft, tol) &&
        IsIn(mousePos.Y, AxisTop, AxisBottom))
        state = state.Add(AdjustType.Left);
      if (ApproxEqual(mousePos.Y, AxisTop, tol) &&
        IsIn(mousePos.X, AxisLeft, AxisRight))
        state = state.Add(AdjustType.Top);
      if (ApproxEqual(mousePos.X, AxisRight, tol) &&
        IsIn(mousePos.Y, AxisTop, AxisBottom))
        state = state.Add(AdjustType.Right);
      if (ApproxEqual(mousePos.Y, AxisBottom, tol) &&
        IsIn(mousePos.X, AxisLeft, AxisRight))
        state = state.Add(AdjustType.Bottom);
      return state;
    }
    private void UpdateCursor(AdjustType state)
    {
      switch (state)
      {
        default:
        case AdjustType.None:
          Cursor = Cursors.Arrow;
          break;
        case AdjustType.Right:
        case AdjustType.Left:
          Cursor = Cursors.SizeWE;
          break;
        case AdjustType.Top:
        case AdjustType.Bottom:
          Cursor = Cursors.SizeNS;
          break;
        case AdjustType.RightBottom:
        case AdjustType.LeftTop:
          Cursor = Cursors.SizeNWSE;
          break;
        case AdjustType.RightTop:
        case AdjustType.LeftBottom:
          Cursor = Cursors.SizeNESW;
          break;
      }
    }
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      var mousePos = e.GetPosition(gridMain);
      State = GetState(mousePos);
      UpdateCursor(State);

      // Initialize Adjust
      if (State != AdjustType.None)
      {
        IsAdjust = true;
        CaptureMouse();

        // block other events
        e.Handled = true;
      }
    }

    private double tol = 10;
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);

      var mousePos = e.GetPosition(gridMain);
      // is not adjusting, just update the cursor
      if (!IsAdjust)
      {
        UpdateCursor(GetState(mousePos));
        return;
      }
      // avoid mouse go outside the grid
      if (!(IsIn(mousePos.X, gridMain.ActualWidth, 0) &&
            IsIn(mousePos.Y, gridMain.ActualHeight, 0)))
        return;
      // adjust 
      if (State.Contain(AdjustType.Left))
      {
        var delta = mousePos.X - AxisLeft;
        AxisLeft = mousePos.X; // must be checked earlier than width
        AxisWidth -= delta;
      }
      if (State.Contain(AdjustType.Top))
      {
        var delta = mousePos.Y - AxisTop;
        AxisTop = mousePos.Y;
        AxisHeight -= delta;
      }
      if (State.Contain(AdjustType.Right))
      {
        AxisWidth = mousePos.X - AxisLeft;
      }
      if (State.Contain(AdjustType.Bottom))
      {
        AxisHeight = mousePos.Y - AxisTop;
      }

    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      IsAdjust = false;
      ReleaseMouseCapture();
    }
    #endregion
  }
}