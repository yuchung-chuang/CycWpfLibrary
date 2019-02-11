using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary
{
  public class VisibilityConverter : ValueConverterBase<VisibilityConverter>
  {
    /// <summary>
    /// Convert <see cref="bool"/> to <see cref="Visibility"/>
    /// </summary>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (Visibility)value == Visibility.Visible ? true : false;
    }
  }
}
