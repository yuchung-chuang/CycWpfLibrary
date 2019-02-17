using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<int> FindAllIndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> finder)
    {
      return enumerable
        .Select((e, i) => finder(e) ? i : -1)
        .Where(i => i != -1);
    }
  }
}
