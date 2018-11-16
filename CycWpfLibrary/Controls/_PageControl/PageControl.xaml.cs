using CycWpfLibrary.MVVM;
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
  /// FrameControl.xaml 的互動邏輯
  /// </summary>
  public partial class PageControl : UserControl
  {
    public PageControl()
    {
      InitializeComponent();

      gridMain.DataContext = this;
      
    }

    public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof(PageManager), typeof(PageManagerBase), typeof(PageControl));
    public PageManagerBase PageManager
    {
      get { return (PageManagerBase)GetValue(CurrentPageProperty); }
      set { SetValue(CurrentPageProperty, value); }
    }
  }
}
