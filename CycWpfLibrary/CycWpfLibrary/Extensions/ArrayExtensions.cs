using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

    public static T[,] RotateClockwise<T>(this T[,] array, int times = 1)
      where T : new()
    {
      for (int i = 0; i < times; i++)
      {
        array = array.RotateClockwise90();
      }
      return array;
    }
    public static T[,,] RotateClockwise<T>(this T[,,] array, int times = 1)
      where T : new()
    {
      for (int i = 0; i < times; i++)
      {
        array = array.RotateClockwise90();
      }
      return array;
    }

    internal static T[,] RotateClockwise90<T>(this T[,] array)
      where T : new()
    {
      (int row, int col) size = (array.GetLength(0), array.GetLength(1));
      var output = new T[size.col, size.row];

      for (int row = 0; row < size.row; row++)
      {
        for (int col = 0; col < size.col; col++)
        {
          output[(size.col - 1) - col, row] = array[row, col];
        }
      }
      return output;
    }
    internal static T[,,] RotateClockwise90<T>(this T[,,] array)
      where T : new()
    {
      (int row, int col, int dep) Length = (array.GetLength(0), array.GetLength(1), array.GetLength(2));
      var output = new T[Length.col, Length.row, Length.dep];
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

    public static T[] Resize<T>(this T[,] array)
    {
      var rowN = array.GetLength(0);
      var colN = array.GetLength(1);
      var output = new T[rowN * colN];
      var idx = 0;
      for (int row = 0; row < rowN; row++)
        for (int col = 0; col < colN; col++)
          output[idx++] = array[row, col];
      return output;
    }
    public static T[] Resize<T>(this T[,,] array)
    {
      var rowN = array.GetLength(0);
      var colN = array.GetLength(1);
      var depN = array.GetLength(2);
      var output = new T[rowN * colN * depN];
      var idx = 0;
      for (int row = 0; row < rowN; row++)
        for (int col = 0; col < colN; col++)
          for (int dep = 0; dep < depN; dep++)
            output[idx++] = array[row, col, dep];
      return output;
    }
    public static T[,] Resize<T>(this T[,] array, int rowN, int colN)
    {
      var vector = array.Resize();
      var output = new T[rowN, colN];
      var idx = 0;
      for (int row = 0; row < rowN; row++)
        for (int col = 0; col < colN; col++)
          output[row, col] = vector[idx++];
      return output;
    }

    public static bool IsEqual<T>(this T[] array1, T[] array2)
    {
      var length = array1.GetLength(0);
      if (length != array2.GetLength(0))
        return false;
      for (int i = 0; i < length; i++)
        if (!array1[i].Equals(array2[i]))
          return false;
      return true;
    }
    public static bool IsEqual<T>(this T[,] array1, T[,] array2)
    {
      var rowN = array1.GetLength(0);
      var colN = array1.GetLength(1);
      if (rowN != array2.GetLength(0))
        return false;
      if (colN != array2.GetLength(1))
        return false;
      for (int row = 0; row < rowN; row++)
        for (int col = 0; col < colN; col++)
          if (!array1[row, col].Equals(array2[row, col]))
            return false;
      return true;

    }

  }
}
