using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CycWpfLibrary.MVVM
{
  /// <summary>
  /// 提供具有<see cref="MarkupExtension"/>功能的基底類別。
  /// </summary>
  /// <typeparam name="ConverterType">要實作的Converter類別。</typeparam>
  public abstract class XamlBase<ConverterType> : MarkupExtension where ConverterType : class, new()
  {
    /// <summary>
    /// <see cref="ConverterType"/>的單例。
    /// </summary>
    private static readonly ConverterType converter = new ConverterType();
    /// <summary>
    /// 提供物件給<see cref="MarkupExtension"/>的覆寫方法。
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return converter;
    }
  }
}
