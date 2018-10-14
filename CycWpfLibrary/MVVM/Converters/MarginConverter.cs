using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CycWpfLibrary.MVVM
{
  public class MarginConverter : MultiValueConverterBase<MarginConverter>
  {
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return new Thickness(System.Convert.ToDouble(values[0]),
                           System.Convert.ToDouble(values[1]),
                           System.Convert.ToDouble(values[2]),
                           System.Convert.ToDouble(values[3]));
    }

    public override object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
