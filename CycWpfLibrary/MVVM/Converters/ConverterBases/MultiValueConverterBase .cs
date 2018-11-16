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
  /// 提供具有<see cref="IMultiValueConverter"/>功能的類別。
  /// </summary>
  /// <typeparam name="MultiValueConverterType">要實作的MultiValueConverter類別。</typeparam>
  public abstract class MultiValueConverterBase<MultiValueConverterType> : XamlMarkupObject<MultiValueConverterType>, IMultiValueConverter 
    where MultiValueConverterType : class, new()
  {
    public abstract object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture);

    public abstract object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture);
  }
}
