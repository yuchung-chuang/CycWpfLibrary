using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CycWpfLibrary
{
  public static class StringExtensions
  {
    public static double ToDouble(this string str) 
      => double.Parse(str);

    public static DateTime ToDate(this string str) 
      => DateTime.Parse(str, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces);

    public static bool IsNull(this string str) 
      => string.IsNullOrEmpty(str);

    public static bool ContainSymbol(this string str)
    {
      var regex = new Regex("[!@#$%^&*(),.?\":{}|<>]");
      var b = regex.IsMatch(str);
      return b;
    }

    public static bool ContainUpper(this string str)
      => str.Any(ch => char.IsUpper(ch));

    public static bool ContainLower(this string str)
      => str.Any(ch => char.IsLower(ch));

    public static bool ContainNumber(this string str)
      => str.Any(ch => char.IsNumber(ch));

    /// <summary>
    /// Compute Damerau–Levenshtein distance for given strings.
    /// </summary>
    public static int EditDistanceTo(this string strA, string strB)
    {
      if (string.IsNullOrEmpty(strA))
      {
        if (!string.IsNullOrEmpty(strB))
          return strB.Length;

        return 0;
      }
      if (string.IsNullOrEmpty(strB))
      {
        if (!string.IsNullOrEmpty(strA))
          return strA.Length;

        return 0;
      }

      var length1 = strA.Length;
      var length2 = strB.Length;

      var d = new int[length1 + 1, length2 + 1];

      int cost, del, ins, sub;

      for (var i = 0; i <= d.GetUpperBound(0); i++)
        d[i, 0] = i;

      for (var i = 0; i <= d.GetUpperBound(1); i++)
        d[0, i] = i;

      for (var i = 1; i <= d.GetUpperBound(0); i++)
      {
        for (var j = 1; j <= d.GetUpperBound(1); j++)
        {
          if (strA[i - 1] == strB[j - 1])
            cost = 0;
          else
            cost = 1;

          del = d[i - 1, j] + 1;
          ins = d[i, j - 1] + 1;
          sub = d[i - 1, j - 1] + cost;

          d[i, j] = System.Math.Min(del, System.Math.Min(ins, sub));

          if (i > 1 && j > 1 && strA[i - 1] == strB[j - 2] && strA[i - 2] == strB[j - 1])
            d[i, j] = System.Math.Min(d[i, j], d[i - 2, j - 2] + cost);
        }
      }

      var distance = d[d.GetUpperBound(0), d.GetUpperBound(1)];
      return distance;
    }

    /// <summary>
    /// Compute the similarity of given strings. 
    /// </summary>
    /// <returns>If the ratio of their EditDistance and average length is greater than given tolerance, the two strings are similar.</returns>
    public static bool IsSimilarTo(this string strA, string strB, double tol = 0.1)
    {
      if (strA.Equals(strB))
        return true;

      strB = strB.ToStringEx(); // null check
      return EditDistanceTo(strA, strB) / (0.5 * (strA.Length + strB.Length)) <= tol;
    }

    public static bool IsNotSimilarTo(this string strA, string strB, double tol = 0.9)
    {
      return !IsSimilarTo(strA, strB, tol);
    }
  }
}
