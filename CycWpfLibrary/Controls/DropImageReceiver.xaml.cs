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
  public partial class DropImageReceiver : UserControl
  {
    public DropImageReceiver()
    {
      InitializeComponent();
    }

    private void grid_Drop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        // Note that you can have more than one file.
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (Path.GetExtension(files[0]))
        {

        } 

        // Only care about the first file
        image.Source = new BitmapImage(new Uri(files[0]));
      }
    }

    private void grid_DragEnter(object sender, DragEventArgs e)
    {

    }

    private void grid_DragLeave(object sender, DragEventArgs e)
    {

    }
  }
}
