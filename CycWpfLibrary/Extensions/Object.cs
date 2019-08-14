using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace CycWpfLibrary
{
  public static class ObjectExtensions
  {
    private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
    
    public static T RecursiveDeepClone<T>(this T obj)
    {
      return (T)RecursiveDeepClone((object)obj);
    }
    public static object RecursiveDeepClone(this object originalObj)
    {
      return InternalCopy(originalObj, new Dictionary<object, object>(new ReferenceEqualityComparer()));

      object InternalCopy(object obj, IDictionary<object, object> visited)
      {
        if (obj == null)
          return null;

        var typeToReflect = obj.GetType();
        if (typeToReflect.IsPrimitive())
          return obj;

        if (visited.ContainsKey(obj))
          return visited[obj];

        if (typeof(Delegate).IsAssignableFrom(typeToReflect))
          return null;

        var cloneObject = CloneMethod.Invoke(obj, null);
        visited.Add(obj, cloneObject);
        CopyFields(obj, visited, cloneObject, typeToReflect);
        RecursiveCopyBaseTypePrivateFields(obj, visited, cloneObject, typeToReflect);
        return cloneObject;
      }

      void RecursiveCopyBaseTypePrivateFields(object obj, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
      {
        if (typeToReflect.BaseType != null)
        {
          RecursiveCopyBaseTypePrivateFields(obj, visited, cloneObject, typeToReflect.BaseType);
          CopyFields(obj, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
        }
      }

      void CopyFields(object obj, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
      {
        foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags))
        {
          if (filter != null && filter(fieldInfo) == false) continue;
          if (fieldInfo.FieldType.IsPrimitive()) continue;
          var originalFieldValue = fieldInfo.GetValue(obj);
          var clonedFieldValue = InternalCopy(originalFieldValue, visited);
          fieldInfo.SetValue(cloneObject, clonedFieldValue);
        }
      }
    }

    /// <summary>
    /// Your class MUST be marked as <see cref="SerializableAttribute"/>
    /// </summary>
    public static T SerializeDeepClone<T>(this T obj)
    {
      using (var ms = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, obj);
        ms.Position = 0;

        return (T)formatter.Deserialize(ms);
      }
    }

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
      return property?.GetValue(obj);
    }
    /// <summary>
    /// Setter with dynamic expression
    /// </summary>
    public static void Set(this object obj, string propertyName, object value)
    {
      var property = obj.GetType().GetProperty(propertyName);
      property?.SetValue(obj, value);
    }
  }
}
