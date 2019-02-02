using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class StringExtensions
  {
    public static double ToDouble(this string str) => double.Parse(str);

    public static DateTime ToDate(this string str) => DateTime.Parse(str, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces);


  }
}
