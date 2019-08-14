using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class TypeExtensions
  {
    public static bool IsPrimitive(this Type type)
    {
      return type == typeof(string) || type.IsValueType & type.IsPrimitive;
    }
  }
}
