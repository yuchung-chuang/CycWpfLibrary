using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Test
{
  public class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
  {
    public string Label { get; set; } = "0";

    int num = 0;
    public RelayCommand ChangeLabelCommand { get; set; }
    private void ChangeLabel(object param)
    {
      Label = (++num).ToString();
    }
    private bool CanChangeLabel(object param)
    {
      return num < 10;
    }

    public static RoutedUICommand Requery { get; set; } = new RoutedUICommand();

    public MainWindowViewModel()
    {
      ChangeLabelCommand = new RelayCommand(ChangeLabel, CanChangeLabel);

      InputGestureCollection inputs = new InputGestureCollection();
      inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
      Requery = new RoutedUICommand("Requery", "Requery", typeof(DataCommands), inputs);
    }

  }
}
