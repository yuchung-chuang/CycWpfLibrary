using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class DictionaryExtensions
  {
    /// <summary>
    /// Set the <see cref="value"/> with checking the existence of <see cref="key"/> 
    /// </summary>
    public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
      if (dict.ContainsKey(key))
        dict[key] = value;
      else
        dict.Add(key, value);
    }

    /// <summary>
    /// Modify the <see cref="change"/> with checking the existence of <see cref="key"/> 
    /// </summary>
    public static void Modify<TKey>(this Dictionary<TKey, int> dict, TKey key, int change)
    {
      if (dict.ContainsKey(key))
        dict[key] += change;
      else
        dict.Add(key, change);
    }
  }
}
