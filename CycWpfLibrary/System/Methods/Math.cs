using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static CycWpfLibrary.NativeMethod;

namespace CycWpfLibrary
{
  public static class Math
  {
    public static int LinConvert(int value1, int max1, int min1, int max2, int min2)
    {
      return (int)Round(LinConvert((double)value1, max1, min1, max2, min2));
    }
    public static float LinConvert(float value1, float max1, float min1, float max2, float min2)
    {
      return (float)LinConvert((double)value1, max1, min1, max2, min2);
    }
    public static double LinConvert(double value1, double max1, double min1, double max2, double min2)
    {
      double r = (max2 - min2) / (max1 - min1);
      return (min2 + (value1 - min1) * r);
    }

    public static float Interpolate(int StartValue, int EndValue, float Ratio)
    {
      return StartValue + Ratio * (EndValue - StartValue);
    }

    public static int Clamp(float value, int Max, int Min)
    {
      return (int)Clamp((double)value, Max, Min);
    }
    public static double Clamp(double value, double Max, double Min)
    {
      if (Min > Max)
        Swap(ref Max, ref Min);

      if (value > Max)
        return Max;
      else if (value < Min)
        return Min;
      else
        return value;
    }

    /// <summary>
    /// 判斷<paramref name="value"/>是否位於閉區間[<paramref name="Max"/>,<paramref name="Min"/>]中。
    /// </summary>
    public static bool IsIn(int value, int Max, int Min)
    {
      return IsIn((double)value, Max, Min);
    }
    /// <summary>
    /// 判斷<paramref name="value"/>是否位於閉區間[<paramref name="Max"/>,<paramref name="Min"/>]中。
    /// </summary>
    public static bool IsIn(float value, int Max, int Min)
    {
      return IsIn((double)value, Max, Min);
    }
    /// <summary>
    /// 判斷<paramref name="value"/>是否位於閉區間[<paramref name="Max"/>,<paramref name="Min"/>]中。<paramref name="excludeBoundary"/>為真時，改為判斷開區間(<paramref name="Max"/>,<paramref name="Min"/>)。
    /// </summary>
    public static bool IsIn(double value, double Max, double Min, bool excludeBoundary = false)
    {
      if (Min > Max)
        Swap(ref Max, ref Min);
      if (!excludeBoundary)
        return (value <= Max && value >= Min) ? true : false;
      else
        return (value < Max && value > Min) ? true : false;
    }

    public static bool IsIn(double value, double Max, double Min, bool excludeMax, bool excludeMin)
    {
      if (Min > Max)
        Swap(ref Max, ref Min);
      var inMax = excludeMax ? value < Max : value <= Max;
      var inMin = excludeMin ? value > Min : value >= Min;
      return inMax && inMin;
    }

    public static double LogBase(double Base, double num)
    {
      return Log(num) / Log(Base);
    }

    /// <summary>
    /// 判斷<paramref name="A"/>是否約等於<paramref name="B"/>。
    /// </summary>
    /// <param name="tol">容許誤差。</param>
    public static bool ApproxEqual(double A, double B, double tol)
    {
      return IsIn(A, B + tol, B - tol);
    }

  }
}
