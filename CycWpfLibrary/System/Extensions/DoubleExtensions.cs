using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary
{
  public static class DoubleExtensions
  {
    public static Duration ToDuration(this double ms)
    {
      return TimeSpan.FromMilliseconds(ms);
    }
  }
}
