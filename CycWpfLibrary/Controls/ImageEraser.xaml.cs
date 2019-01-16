using CycWpfLibrary.Emgu;
using CycWpfLibrary.MVVM;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CycWpfLibrary.Geometry;
using CycWpfLibrary.WinForm;
using System;

namespace CycWpfLibrary.Controls
{
  /// <summary>
  /// ImageEditor.xaml 的互動邏輯
  /// </summary>
  public partial class ImageEraser : ViewModelUserControl
  {
    public ImageEraser()
    {
      InitializeComponent();
      mainGrid.DataContext = this;
    }

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
      nameof(Image), 
      typeof(Image<Bgra, byte>), 
      typeof(ImageEraser),
      new PropertyMetadata(ImageChanged));

    private static void ImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as ImageEraser).OnPropertyChanged(nameof(ImageSource));
    }

    public Image<Bgra, byte> Image
    {
      get => GetValue(ImageProperty) as Image<Bgra, byte>;
      set => SetValue(ImageProperty, value);
    }

    public BitmapSource ImageSource => Image?.ToBitmapSource();

    private Point mousePos;
    private ScaleTransform scale => panZoom.scale;
    private double eraserSize => 20 / scale.ScaleX;
    private bool isEdit;

    private void image_MouseEnter(object sender, MouseEventArgs e)
    {
      image.Cursor = null;
      rectCursor.Visibility = Visibility.Visible;
    }
    private void image_MouseLeave(object sender, MouseEventArgs e)
    {
      Cursor = Cursors.Arrow;
      rectCursor.Visibility = Visibility.Collapsed;
    }
    private void image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      isEdit = false;
      image.CaptureMouse();
      image_MouseMove(sender, e);
    }
    private void image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
      image.ReleaseMouseCapture();
      if (isEdit)
      {
        Image = Image; //invoke twoway binding for Image
      }
    }
    private void image_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      UpdateCursor();
    }

    private void image_MouseMove(object sender, MouseEventArgs e)
    {
      mousePos = e.GetPosition(image);
      if (e.RightButton == MouseButtonState.Pressed && image.IsMouseCaptured)
      {
        // Erase Image
        CvInvoke.Rectangle(Image, new Rect(mousePos.Minus(new Point(eraserSize / 2, eraserSize / 2)), new Vector(eraserSize, eraserSize)).ToWinForm(), Color.FromArgb(0, 0, 0, 0).ToMCvScalar(), -1);
        OnPropertyChanged(nameof(ImageSource));
        isEdit = true;
      }
      UpdateCursor();
    }

    private void UpdateCursor()
    {
      rectCursor.Width = eraserSize;
      rectCursor.Height = eraserSize;
      Canvas.SetLeft(rectCursor, mousePos.X - eraserSize / 2);
      Canvas.SetTop(rectCursor, mousePos.Y - eraserSize / 2);
    }
  }
}
