using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary.Linq.Expressions
{
  /// <summary>
  /// 提供<see cref="Expression"/>的擴充方法。
  /// </summary>
  public static class ExpressionExtensions
  {
    /// <summary>
    /// 評估<paramref name="lambda"/>運算式並回傳結果。
    /// </summary>
    /// <typeparam name="T">回傳結果的類型。</typeparam>
    /// <param name="lambda">要評估的運算式。</param>
    public static T Evaluate<T>(this Expression<Func<T>> lambda)
    {
      return lambda.Compile().Invoke();
    }
  }
}
