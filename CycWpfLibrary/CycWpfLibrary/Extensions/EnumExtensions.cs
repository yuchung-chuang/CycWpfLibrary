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
    public static dynamic Add(this Enum enumA, Enum enumB)
    {
      var (a, b) = ConvertEnums(enumA, enumB);
      return a | b;
    }

    public static dynamic Remove(this Enum enumA, Enum enumB)
    {
      var (a, b) = ConvertEnums(enumA, enumB);
      return enumA.Contain(enumB) ? a ^ b : a;
    }

    public static bool Contain(this Enum enumA, Enum enumB)
    {
      var (a, b) = ConvertEnums(enumA, enumB);
      return (a & b) == b;
    }

    public static bool GreaterThan(this Enum enumA, Enum enumB)
    {
      var (a, b) = ConvertEnums(enumA, enumB);
      return a > b;
    }

    public static bool LessThan(this Enum enumA, Enum enumB)
    {
      var (a, b) = ConvertEnums(enumA, enumB);
      return a < b;
    }

    #region Helper
    private static void CheckType(Enum enumA, Enum enumB)
    {
      if (enumA.GetType() != enumB.GetType())
        throw new ArgumentException("Type Mismatch");
    }
    private static (ulong a, ulong b) ConvertEnums(Enum enumA, Enum enumB) => (Convert.ToUInt64(enumA), Convert.ToUInt64(enumB));
    #endregion
  }
}
