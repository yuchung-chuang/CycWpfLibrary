using CycWpfLibrary.Emgu;
using CycWpfLibrary.Media;
using CycWpfLibrary.MVVM;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static CycWpfLibrary.Math;

namespace CycWpfLibrary.UserControls
{
  public enum AdjustType
  {
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    LeftTop = Left | Top,
    RightTop = Right | Top,
    LeftBottom = Left | Bottom,
    RightBottom = Right | Bottom,
  }
  /// <summary>
  /// Axis.xaml 的互動邏輯
  /// </summary>
  public partial class Axis : ObservableUserControl
  {
    public Axis()
    {
      InitializeComponent();
      gridMain.DataContext = this;
    }

    #region DPs
    public Brush Stroke
    {
      get => (Brush)GetValue(StrokeProperty);
      set => SetValue(StrokeProperty, value);
    }
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
        nameof(Stroke),
        typeof(Brush),
        typeof(Axis),
        new PropertyMetadata(new SolidColorBrush(Colors.Green)));

    public double AxisLeft
    {
      get => (double)GetValue(AxisLeftProperty);
      set => SetValue(AxisLeftProperty, Clamp(value, AxisRight - tol, 0));
    }
    public static readonly DependencyProperty AxisLeftProperty = DependencyProperty.Register(
        nameof(AxisLeft),
        typeof(double),
        typeof(Axis),
        new PropertyMetadata(default(double), OnAxisLeftChanged));

    public double AxisTop
    {
      get => (double)GetValue(AxisTopProperty);
      set => SetValue(AxisTopProperty, Clamp(value, AxisBottom - tol, 0));
    }
    public static readonly DependencyProperty AxisTopProperty = DependencyProperty.Register(
        nameof(AxisTop),
        typeof(double),
        typeof(Axis),
        new PropertyMetadata(default(double), OnAxisTopChanged));

    public double AxisWidth
    {
      get => (double)GetValue(AxisWidthProperty);
      set => SetValue(AxisWidthProperty, Clamp(value, double.MaxValue, tol));
    }
    public static readonly DependencyProperty AxisWidthProperty = DependencyProperty.Register(
        nameof(AxisWidth),
        typeof(double),
        typeof(Axis),
        new PropertyMetadata(default(double), OnAxisWidthChanged));
    
    public double AxisHeight
    {
      get => (double)GetValue(AxisHeightProperty);
      set => SetValue(AxisHeightProperty, Clamp(value, double.MaxValue, tol));
    }
    public static readonly DependencyProperty AxisHeightProperty = DependencyProperty.Register(
        nameof(AxisHeight),
        typeof(double),
        typeof(Axis),
        new PropertyMetadata(default(double), OnAxisHeightChanged));
    public ImageSource ImageSource
    {
      get => (ImageSource)GetValue(ImageSourceProperty);
      set => SetValue(ImageSourceProperty, value);
    }
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(Axis),
        new PropertyMetadata(default(ImageSource), OnImageSourceChanged));

    public CycInput Input
    {
      get => (CycInput)GetValue(InputProperty);
      set => SetValue(InputProperty, value);
    }
    public static readonly DependencyProperty InputProperty = DependencyProperty.Register(
        nameof(Input),
        typeof(CycInput),
        typeof(Axis),
        new PropertyMetadata(new CycInput()));

    public CycInputCollection Inputs
    {
      get => (CycInputCollection)GetValue(InputsProperty);
      set => SetValue(InputsProperty, value);
    }
    public static readonly DependencyProperty InputsProperty = DependencyProperty.Register(
        nameof(Inputs),
        typeof(CycInputCollection),
        typeof(Axis),
        new PropertyMetadata(new CycInputCollection()));
    #endregion
    private static void OnAxisLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var axis = d as Axis;
      axis.OnPropertyChanged(nameof(AxisMargin));
      axis.OnPropertyChanged(nameof(AxisRelative));
    }
    private static void OnAxisTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var axis = d as Axis;
      axis.OnPropertyChanged(nameof(AxisMargin));
      axis.OnPropertyChanged(nameof(AxisRelative));
    }
    private static void OnAxisWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var axis = d as Axis;
      axis.OnPropertyChanged(nameof(AxisRelative));
    }
    private static void OnAxisHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var axis = d as Axis;
      axis.OnPropertyChanged(nameof(AxisRelative));
    }
    private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var axis = d as Axis;
      axis.image = (axis.ImageSource as BitmapSource).ToPixelBitmap();
      axis.OnPropertyChanged(nameof(AxisRelative));
    }

    public Thickness AxisMargin => new Thickness(AxisLeft, AxisTop, 0, 0);
    public Rect AxisRelative => image == null ? new Rect() : new Rect(AxisLeft / image.Width, AxisTop / image.Height, AxisWidth / image.Width, AxisHeight / image.Height);
    public double AxisRight => AxisLeft + AxisWidth;
    public double AxisBottom => AxisTop + AxisHeight;

    private PixelBitmap image;
    private double tol = 10;
    private bool IsAdjust = false;
    private AdjustType State = AdjustType.None;
    private AdjustType GetState(Point mousePos)
    {
      var state = new AdjustType();
      if (ApproxEqual(mousePos.X, AxisLeft, tol) &&
        IsIn(mousePos.Y, AxisBottom + tol, AxisTop - tol))
        state = (AdjustType)state.Add(AdjustType.Left);
      if (ApproxEqual(mousePos.Y, AxisTop, tol) &&
        IsIn(mousePos.X, AxisRight + tol, AxisLeft - tol))
        state = (AdjustType)state.Add(AdjustType.Top);
      if (ApproxEqual(mousePos.X, AxisRight, tol) &&
        IsIn(mousePos.Y, AxisBottom + tol, AxisTop - tol))
        state = (AdjustType)state.Add(AdjustType.Right);
      if (ApproxEqual(mousePos.Y, AxisBottom, tol) &&
        IsIn(mousePos.X, AxisRight + tol, AxisLeft - tol))
        state = (AdjustType)state.Add(AdjustType.Bottom);
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
        case AdjustType.Left:
        case AdjustType.Right:
          Cursor = Cursors.SizeWE;
          break;
        case AdjustType.Top:
        case AdjustType.Bottom:
          Cursor = Cursors.SizeNS;
          break;
        case AdjustType.LeftTop:
        case AdjustType.RightBottom:
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
      if (!InputCheck(e))
        return;
      var mousePos = e.GetPosition(gridMain);
      State = GetState(mousePos);
      UpdateCursor(State);

      // Initialize Adjust
      if (State != (AdjustType.None))
      {
        IsAdjust = true;
        CaptureMouse();

        // block other events
        e.Handled = true;
      }
    }
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      IsAdjust = false;
      ReleaseMouseCapture();
    }
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
        AxisWidth = mousePos.X - AxisLeft;
      if (State.Contain(AdjustType.Bottom))
        AxisHeight = mousePos.Y - AxisTop;
    }

    private bool InputCheck(EventArgs e)
    {
      var arg = e is MouseButtonEventArgs mbe ? mbe : null;
      return (!Input.IsEmpty && Input.IsValid(arg)) || (!Inputs.IsEmpty && Inputs.IsValid(arg)) ? true : false;
    }
  }
}
