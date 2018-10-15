using CycWpfLibrary.Media;
using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace CycWpfLibrary.Controls
{
  /// <summary>
  /// DropImageReceiver.xaml 的互動邏輯
  /// </summary>
  public partial class DropImageReceiver : ViewModelUserControl
  {
    private readonly string brushResource = "ShadowBrush";
    private SolidColorBrush ShadowBrush;
    public DropImageReceiver()
    {
      InitializeComponent();
      ShadowBrush = FindResource(brushResource) as SolidColorBrush;

      grid.DataContext = this;
    }

    // Can't be fired by OnPropertyChanged???
    public BitmapSource DropImageSource => DropPixelBitmap?.ToBitmapSource();

    public static readonly DependencyProperty DropPixelBitmapProperty = DependencyProperty.Register(nameof(DropPixelBitmap), typeof(PixelBitmap), typeof(DropImageReceiver));
    public PixelBitmap DropPixelBitmap
    {
      get => (PixelBitmap)GetValue(DropPixelBitmapProperty);
      set => SetValue(DropPixelBitmapProperty, value);
    }

    private void grid_Drop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;

      // Note that you can have more than one file.
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

      if (ImageExtensions.List.Contains(Path.GetExtension(files[0])))
      {
        // Only care about the first file
        DropPixelBitmap = new BitmapImage(new Uri(files[0])).ToPixelBitmap();
      }
      else
      {
        MessageBox.Show("Input file should be an image.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      border.Background = new SolidColorBrush(Colors.Transparent);
    }

    private void grid_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;

      // color animation
      border.Background = ShadowBrush;
    }

    private void grid_DragLeave(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;

      border.Background = new SolidColorBrush(Colors.Transparent);
    }

    private void grid_MouseDown(object sender, MouseButtonEventArgs e)
    {

    }
  }
}
