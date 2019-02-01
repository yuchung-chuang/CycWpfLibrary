using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CycWpfLibrary.Resources
{
  public static class ResourceManager
  {
    public static readonly Cursor panCursor = new Cursor(Application.GetResourceStream(new Uri(@"pack://application:,,,/CycWpfLibrary.Resources;component/pan.cur", UriKind.Absolute)).Stream);
  }
}
