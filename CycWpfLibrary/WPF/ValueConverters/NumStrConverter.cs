using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary
{
  public class NumStrConverter : ValueConverterBase<NumStrConverter>
  {
    /// <summary>
    /// Convert double to string.
    /// </summary>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
      (value ?? "").ToString();

    /// <summary>
    /// Convert number string to double.
    /// </summary>
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value ?? "").ToString().IsNull() ? null : (double?)double.Parse(value.ToString());
  }
}
