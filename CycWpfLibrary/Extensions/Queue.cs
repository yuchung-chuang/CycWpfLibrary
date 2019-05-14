using System.Collections.Generic;

namespace CycWpfLibrary
{
  public static class QueueExtensions
  {
    public static void AddRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
      foreach (var item in items)
        queue.Enqueue(item);
    }
  }
}
