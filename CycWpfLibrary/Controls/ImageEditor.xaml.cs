using CycWpfLibrary.Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
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

namespace CycWpfLibrary.Controls
{
  /// <summary>
  /// ImageEditor.xaml 的互動邏輯
  /// </summary>
  public partial class ImageEditor : UserControl
  {
    public ImageEditor()
    {
      InitializeComponent();
      mainGrid.DataContext = this;
    }

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(Image<Bgra, byte>), typeof(ImageEditor));
    public Image<Bgra, byte> Image
    {
      get => GetValue(ImageProperty) as Image<Bgra, byte>;
      set => SetValue(ImageProperty, value);
    }

    public BitmapSource ImageSource => Image?.ToBitmapSource();
  }
}
