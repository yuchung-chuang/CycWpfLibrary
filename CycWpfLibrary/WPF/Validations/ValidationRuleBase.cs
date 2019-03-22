using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public abstract class ValidationRuleBase : ValidationRule
  {
    public string Message { get; set; }
  }
}
