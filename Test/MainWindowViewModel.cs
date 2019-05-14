using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CycWpfLibrary.MVVM;

namespace Test
{
  public class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      ChangeLabelCommand = new RelayCommand<object>(ChangeLabel, CanChangeLabel);
      RequeryCommand = DataCommands.Requery;
    }

    public string Label { get; set; } = "0";
    int num = 0;

    public static ICommand ChangeLabelCommand { get; set; }
    public static ICommand RequeryCommand { get; set; }

    private void ChangeLabel(object param)
    {
      Label = (++num).ToString();
    }
    private bool CanChangeLabel(object param)
    {
      return num < 10;
    }

  }
}
