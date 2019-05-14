using System;
using System.Collections.Generic;
using System.Linq;

namespace CycWpfLibrary
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
      return enumerable.OrderBy(_ => Math.RandInt());
    }

    public static IEnumerable<int> FindAllIndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> match)
    {
      return enumerable
        .Select((e, i) => match(e) ? i : -1)
        .Where(i => i != -1);
    }

    public static T FindMax<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
    {
      var max = enumerable.Max(selector);
      return enumerable.First(e => selector(e) == max);
    }
  }
}
