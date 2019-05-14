using System;
using System.Linq;
using System.Reflection;

namespace CycWpfLibrary
{
  public static class ObjectExtensions
  {
    public static bool IsNumeric(this object obj)
    {
      switch (Type.GetTypeCode(obj.GetType()))

      {
        case TypeCode.Byte:
        case TypeCode.SByte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Decimal:
        case TypeCode.Double:
        case TypeCode.Single:
          return true;
        default:
          return false;
      }
    }
    /// <summary>
    /// return empty string if object is null
    /// </summary>
    public static string ToStringEx(this object obj)
    {
      return (obj ?? "").ToString();
    }

    /// <summary>
    /// Get <see cref="PropertyInfo"/> by type <typeparamref name="TProperty"/>
    /// </summary>
    public static PropertyInfo GetPropInfo<TProperty>(this Type type)
    {
      return type.GetProperties()
        .First(p => p.PropertyType == typeof(TProperty));
    }
    /// <summary>
    /// Get property of <paramref name="obj"/> by type <typeparamref name="TProperty"/>
    /// </summary>
    public static TProperty Get<TProperty>(this object obj)
      where TProperty : class
    {
      return obj.GetType()
        .GetPropInfo<TProperty>()
        .GetValue(obj) as TProperty;
    }
    /// <summary>
    /// Get property of <paramref name="type"/> by type <typeparamref name="TProperty"/>
    /// </summary>
    /// <remarks>Use for static class or static method</remarks>
    public static TProperty Get<TProperty>(this Type type)
      where TProperty : class
    {
      return type.GetPropInfo<TProperty>()
        .GetValue(null) as TProperty;
    }

    public static void Set<TProperty>(this object obj, TProperty value)
    {
      obj.GetType()
        .GetPropInfo<TProperty>()
        .SetValue(obj,value);
    }
    public static void Set<TProperty>(this Type type, TProperty value)
      where TProperty : class
    {
      type.GetPropInfo<TProperty>()
        .SetValue(null, value);
    }


    /// <summary>
    /// Getter with dynamic expression
    /// </summary>
    public static object Get(this object obj, string propertyName)
    {
      var property = obj.GetType().GetProperty(propertyName);
      return property.GetValue(obj);
    }
    /// <summary>
    /// Setter with dynamic expression
    /// </summary>
    public static void Set(this object obj, string propertyName, object value)
    {
      var property = obj.GetType().GetProperty(propertyName);
      property.SetValue(obj, value);
    }
  }
}
