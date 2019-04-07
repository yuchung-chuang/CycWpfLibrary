using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public class StaticPropertyChangedEventArgs : EventArgs
  {
    public StaticPropertyChangedEventArgs(string className, string propertyName)
    {
      ClassName = className;
      PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
    public string ClassName { get; set; }
  }
}
