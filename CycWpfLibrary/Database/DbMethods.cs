using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
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
