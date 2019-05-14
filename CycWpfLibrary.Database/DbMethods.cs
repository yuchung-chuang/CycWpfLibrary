using System.Collections.Generic;
using System.Linq;

namespace CycWpfLibrary.Database
{
  public static class DbMethods
  {
    public static bool DataEquals<TData>(this List<TData> dataA, List<TData> dataB) 
      where TData : IData
    {
      if (dataA.Count == 0 || dataB.Count == 0)
        return false;
      return dataA.Max(d => d.ID) == dataB.Max(d => d.ID);
    }
  }
}
