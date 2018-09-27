using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
    /// 評估<see cref="LambdaExpression"/>並取得表達式的屬性值。
    /// </summary>
    public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
    {
      return lambda.Compile().Invoke();
    }

    public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
    {
      //取得表達式
      var expression = (lambda as LambdaExpression).Body as MemberExpression;
      //取得表達式的屬性資訊
      var propertyInfo = expression.Member as PropertyInfo;
      //取得表達式左方的目標
      var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

      propertyInfo.SetValue(target, value);
    }
  }
}
