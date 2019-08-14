using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public static class CollectionExtensions
  {
    public static void Remove<T>(this Collection<T> collection, Func<T, bool> match)
    {
      collection.Remove(collection.FirstOrDefault(match));
    }

    public static void RemoveAll<T>(this Collection<T> collection, Func<T, bool> match)
    {
      var list = collection.ToList();
      var removeList = list.Where(match);
      foreach (var item in removeList)
      {
        collection.Remove(item);
      }
    }

    public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> addList)
    {
      foreach (var item in addList)
      {
        collection.Add(item);
      }
    }
  }
}
