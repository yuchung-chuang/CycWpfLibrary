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
  /// 提供具有<see cref="IValueConverter"/>以及<see cref="MarkupExtension"/>功能的類別。
  /// </summary>
  /// <typeparam name="ValueConverterType">要實作的ValueConverter類別。</typeparam>
  public abstract class ValueConverterBase<ValueConverterType> : MarkupExtension, IValueConverter 
    where ValueConverterType : class, new()
  {
    static ValueConverterType converter;
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return converter ?? (converter = new ValueConverterType());
    }

    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
  }
}
