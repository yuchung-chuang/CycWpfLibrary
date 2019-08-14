using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class NumberExtensions
  {
    /// <summary>
    /// Set the <see cref="index"/>-th bit in <see cref="num"/> to 1
    /// </summary>
    public static long BitSet(this long num, int index)
    {
      return num |= 1L << index;
    }

    /// <summary>
    /// Set the <see cref="index"/>-th bit in <see cref="num"/> to 0
    /// </summary>
    public static long BitClear(this long num, int index)
    {
      return num &= ~(1L << index);
    }

    /// <summary>
    /// Toggle the <see cref="index"/>-th bit in <see cref="num"/> 
    /// </summary>
    public static long BitToggle(this long num, int index)
    {
      return num ^= 1L << index;
    }

    /// <summary>
    /// Test if the <see cref="index"/>-th bit in <see cref="num"/> is 1
    /// </summary>
    public static bool BitTest(this long num, int index)
    {
      return (num & (1L << index)) != 0;
    }


  }
}
