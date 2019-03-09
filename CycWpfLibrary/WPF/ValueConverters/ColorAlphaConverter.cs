using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CycWpfLibrary
{
  public class ColorAlphaConverter : ValueConverterBase<ColorAlphaConverter>
  {
    /// <summary>
    /// Convert the alpha of input color. 
    /// </summary>
    /// <param name="value">Color to be converted</param>
    /// <param name="parameter">The ratio of alpha (0~1)</param>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var color = (Color)value;
      var alphaRatio = double.Parse(parameter.ToString());
      color.A = (byte)(alphaRatio * 255);
      return color;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
