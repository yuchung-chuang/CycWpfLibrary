using System;
using System.Globalization;

namespace CycWpfLibrary
{
  public class NumStrConverter : ValueConverterBase<NumStrConverter>
  {
    /// <summary>
    /// Convert number to string.
    /// </summary>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();

    /// <summary>
    /// Convert number string to double.
    /// </summary>
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value ?? "").ToString().IsNull() ? null : (double?)double.Parse(value.ToString());
  }
}
