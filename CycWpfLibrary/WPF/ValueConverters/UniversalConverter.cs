using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public class UniversalConverter : ValueConverterBase<UniversalConverter>
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      // obtain the conveter for the target type
      TypeConverter converter = TypeDescriptor.GetConverter(targetType);

      try
      {
        // determine if the supplied value is of a suitable type
        if (converter.CanConvertFrom(value.GetType()))
        {
          // return the converted value
          return converter.ConvertFrom(value);
        }
        else
        {
          // try to convert from the string representation
          return converter.ConvertFrom(value.ToString());
        }
      }
      catch (Exception)
      {
        Debugger.Break();
        return value;
      }

    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
