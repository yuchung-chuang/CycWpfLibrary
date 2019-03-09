using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary.FluentDesign
{
  public class OpacityConverter : MultiValueConverterBase<OpacityConverter>
  {
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values[0] == DependencyProperty.UnsetValue)
        return 0;
      var opacity = double.Parse(values[0].ToString());
      var isEnter = (bool)values[1];
      return isEnter ? opacity : 0;
    }

    public override object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
