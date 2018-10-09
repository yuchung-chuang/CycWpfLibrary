using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public static class ArrayExtensions
  {
    public static (int, int) IndexOf(this int[,] array, int value)
    {
      var L1 = array.GetLength(0);
      var L2 = array.GetLength(1);
      for (int i = 0; i < L1; i++)
      {
        for (int j = 0; j < L2; j++)
        {
          var item = array[i, j];
          if (item == value)
          {
            return (i, j);
          }
        }
      }
      //didn't find...
      return (-1, -1);
    }

    public static byte[,] RotateClockwise(this byte[,] array, int times = 1)
    {
      for (int i = 0; i < times; i++)
      {
        array = array.RotateClockwise90();
      }
      return array;
    }
    public static byte[,,] RotateClockwise(this byte[,,] array, int times = 1)
    {
      for (int i = 0; i < times; i++)
      {
        array = array.RotateClockwise90();
      }
      return array;
    }

    internal static byte[,] RotateClockwise90(this byte[,] array)
    {
      (int row, int col) size = (array.GetLength(0), array.GetLength(1));
      var output = new byte[size.col, size.row];

      for (int row = 0; row < size.row; row++)
      {
        for (int col = 0; col < size.col; col++)
        {
          output[(size.col - 1) - col, row] = array[row, col];
        }
      }
      return output;
    }
    internal static byte[,,] RotateClockwise90(this byte[,,] array)
    {
      (int row, int col, int dep) Length = (array.GetLength(0), array.GetLength(1), array.GetLength(2));
      var output = new byte[Length.col, Length.row, Length.dep];
      for (int dep = 0; dep < Length.dep; dep++)
      {
        for (int row = 0; row < Length.row; row++)
        {
          for (int col = 0; col < Length.col; col++)
          {
            output[(Length.col - 1) - col, row, dep] = array[row, col, dep];
          }
        }
      }
      return output;
    }
  }
}
