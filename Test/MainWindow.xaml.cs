using System.Windows;
using System.Windows.Input;

namespace Test
{
  /// <summary>
  /// MainWindow.xaml 的互動邏輯
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Requery_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      // 按下按鈕或按下鍵盤Ctrl+R都會觸發
    }
  }
}
