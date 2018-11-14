using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class EnumExtensions
  {
    private static void CheckType(Enum enumA, Enum enumB)
    {
      if (enumA.GetType() != enumB.GetType())
        throw new ArgumentException("Type Mismatch");
    }

    public static dynamic Add(this Enum enumA, Enum enumB)
    {
      CheckType(enumA, enumB);
      return Convert.ToUInt64(enumA) | Convert.ToUInt64(enumB);
    }

    public static dynamic Remove(this Enum enumA, Enum enumB)
    {
      CheckType(enumA, enumB);
      return Convert.ToUInt64(enumA) ^ Convert.ToUInt64(enumB);
    }

    public static bool Contain(this Enum enumA, Enum enumB)
    {
      CheckType(enumA, enumB);
      var a = Convert.ToUInt64(enumA);
      var b = Convert.ToUInt64(enumB);
      return (a & b) == b;
    }
  }
}
