using System;
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
