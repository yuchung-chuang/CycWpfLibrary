using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class PointerExtensions
  {
    /// <summary>
    /// Gets high bits values of the pointer.
    /// </summary>
    public static int GetHIWORD(this IntPtr ptr) => (ptr.ToInt32() >> 16) & 0xFFFF;

    /// <summary>
    /// Gets low bits values of the pointer.
    /// </summary>
    public static int GetLOWORD(this IntPtr ptr) => ptr.ToInt32() & 0xFFFF;
  }
}
