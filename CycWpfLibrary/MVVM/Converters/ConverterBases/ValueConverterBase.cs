using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供具有<see cref="IValueConverter"/>功能的類別。
  /// </summary>
  /// <typeparam name="ValueConverterType">要實作的ValueConverter類別。</typeparam>
  public abstract class ValueConverterBase<ValueConverterType> : ConverterBase<ValueConverterType>, IValueConverter where ValueConverterType : class, new()
  {
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
  }
}
