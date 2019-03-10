using CycWpfLibrary;
using Bitmap = System.Drawing.Bitmap;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CycWpfLibrary.FluentDesign;

namespace Test
{
  /// <summary>
  /// MainWindow.xaml 的互動邏輯
  /// </summary>
  public partial class MainWindow : RevealWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Requery_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      // 按下按鈕或按下鍵盤Ctrl+R都會觸發
      var n = int.Parse(label1.Content as string);
      label1.Content = (n++).ToString();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      //< Rectangle Name = "rectCursor"
      //           Width = "20"
      //           Height = "20"
      //           Fill = "#7F00FFFF"
      //           Stroke = "Black"
      //           StrokeThickness = "2"
      //           IsHitTestVisible = "False" />
      var rect = new Rectangle
      {
        Width = 20,
        Height = 20,
        Fill = new SolidColorBrush(Color.FromArgb(10, 0, 255, 255)),
        Stroke = new SolidColorBrush(Colors.Black),
        StrokeThickness = 2,
        IsHitTestVisible = false,
      };
      this.Cursor = rect.ToCursor(new Point(0.5, 0.5));
    }

    private void WindowButton_Click(object sender, RoutedEventArgs e)
    {
      var window = new Window1();
      window.ShowDialog();
    }
  }
}
