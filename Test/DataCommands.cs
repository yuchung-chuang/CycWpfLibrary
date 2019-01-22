using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Test
{
  public class DataCommands
  {
    public static RoutedUICommand Requery { get; private set; }

    static DataCommands()
    {
      InputGestureCollection inputs = new InputGestureCollection();
      inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
      Requery = new RoutedUICommand("Requery", "Requery", typeof(DataCommands), inputs);
    }

  }
}
