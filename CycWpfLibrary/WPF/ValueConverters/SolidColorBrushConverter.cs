using System;
using System.Globalization;
using System.Windows.Media;

namespace CycWpfLibrary
{
  public class ColorAlphaBrushConverter : ValueConverterBase<ColorAlphaBrushConverter>
  {
    /// <summary>
    /// Convert the alpha of input <see cref="Color"/> and return its <see cref="SolidColorBrush"/> 
    /// </summary>
    /// <param name="value">Color to be converted</param>
    /// <param name="parameter">The ratio of alpha (0~1)</param>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var alphaConverter = new ColorAlphaConverter();
      var color = alphaConverter.Convert(value, targetType, parameter, culture);
      return new SolidColorBrush((Color)color);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
