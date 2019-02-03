using CycWpfLibrary.Emgu;
using CycWpfLibrary.Input;
using CycWpfLibrary.Media;
using CycWpfLibrary.MVVM;
using CycWpfLibrary.WinForm;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.UserControls
{
  /// <summary>
  /// ImageEditor.xaml 的互動邏輯
  /// </summary>
  public partial class ImageEraser : ObservableUserControl
  {
    public ImageEraser()
    {
      InitializeComponent();
      mainGrid.DataContext = this;
      this.EnsureTransforms();
      scale = (RenderTransform as TransformGroup).Children.GetScale();
      RenderOptions.SetBitmapScalingMode(imageControl, BitmapScalingMode.LowQuality);
    }

    #region Dependency Properties
    public Image<Bgra, byte> Image
    {
      get => GetValue(ImageProperty) as Image<Bgra, byte>;
      set => SetValue(ImageProperty, value);
    }
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
      nameof(Image),
      typeof(Image<Bgra, byte>),
      typeof(ImageEraser),
      new PropertyMetadata(ImageChanged));

    public MouseButton MouseButton
    {
      get => (MouseButton)GetValue(MouseButtonProperty);
      set => SetValue(MouseButtonProperty, value);
    }
    public static readonly DependencyProperty MouseButtonProperty = DependencyProperty.Register(
        nameof(MouseButton),
        typeof(MouseButton),
        typeof(ImageEraser),
        new PropertyMetadata(default(MouseButton)));

    #endregion

    private static void ImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var element = d as ImageEraser;
      element.ImageDisplay = element.Image.Clone();
    }

    private Image<Bgra, byte> imageDisplay;
    public Image<Bgra, byte> ImageDisplay
    {
      get => imageDisplay;
      set
      {
        imageDisplay = value;
        OnPropertyChanged(nameof(ImageSource));
      }
    }
    /// <summary>
    /// 不要直接將BitmapSource綁定到DependencyProperty上，會多呼叫GetValue()而降低效率
    /// 因此這裡使用一個public property來當作中間層
    /// </summary>
    public BitmapSource ImageSource => imageDisplay?.ToBitmapSource();

    private Point mousePos;
    private ScaleTransform scale;
    private double eraserSize => 20 / scale.ScaleX;
    private bool isEdit;

    private void image_MouseEnter(object sender, MouseEventArgs e)
    {
      rectCursor.Visibility = Visibility.Visible;
    }
    private void image_MouseLeave(object sender, MouseEventArgs e)
    {
      rectCursor.Visibility = Visibility.Collapsed;
    }
    private void image_MouseDown(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (MouseButton.IsPressed())
      {
        isEdit = false;
        imageControl.CaptureMouse();
        stopwatch.Restart();
        image_MouseMove(sender, e);
      }
    }
    private void image_MouseUp(object sender, MouseButtonEventArgs e)
    {
      imageControl.ReleaseMouseCapture();
      if (isEdit)
        Image = imageDisplay; //invoke Image.set -> two-way binding 
    }

    private Stopwatch stopwatch = new Stopwatch();
    private double fps = 24;
    private async void image_MouseMove(object sender, MouseEventArgs e)
    {
      mousePos = e.GetPosition(imageControl);
      if (MouseButton.IsPressed())
      {
        imageDisplay = await imageDisplay.EraseImageAsync(new Rect(
          mousePos.Minus(new Point(eraserSize / 2, eraserSize / 2)),
          new Vector(eraserSize, eraserSize))); // 不更新畫面
        if (stopwatch.ElapsedMilliseconds > 1000 / fps)
        {
          OnPropertyChanged(nameof(ImageSource)); //每隔fps才更新一次畫面
          stopwatch.Restart();
        }
        isEdit = true;
      }
      UpdateCursor();
    }
    private void image_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      UpdateCursor();
    }

    private void UpdateCursor()
    {
      rectCursor.Width = eraserSize;
      rectCursor.Height = eraserSize;
      rectCursor.StrokeThickness = eraserSize / 10;
      Canvas.SetLeft(rectCursor, mousePos.X - eraserSize / 2);
      Canvas.SetTop(rectCursor, mousePos.Y - eraserSize / 2);
    }

  }
}
