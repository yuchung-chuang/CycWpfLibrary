using System;
using System.Collections.Generic;
using System.Linq;

namespace CycWpfLibrary
{
  public static class ArrayExtensions
  {
    /// <summary>
    /// Find the index of <paramref name="value"/> in <paramref name="array"/>
    /// </summary>
    /// <typeparam name="T">The type of array. Note that it should implement <see cref="object.Equals(object)"/> method so as the comparasion is correct.</typeparam>
    public static (int, int) IndexOf<T>(this T[,] array, T value)
    {
      var L1 = array.GetLength(0);
      var L2 = array.GetLength(1);
      for (var i = 0; i < L1; i++)
      {
        for (var j = 0; j < L2; j++)
        {
          var item = array[i, j];
          if (item.Equals(value))
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
      for (var i = 0; i < times; i++)
      {
        array = array.RotateClockwise90();
      }
      return array;
    }
    public static T[,,] RotateClockwise<T>(this T[,,] array, int times = 1)
      where T : new()
    {
      for (var i = 0; i < times; i++)
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

      for (var row = 0; row < size.row; row++)
      {
        for (var col = 0; col < size.col; col++)
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
      for (var dep = 0; dep < Length.dep; dep++)
      {
        for (var row = 0; row < Length.row; row++)
        {
          for (var col = 0; col < Length.col; col++)
          {
            output[(Length.col - 1) - col, row, dep] = array[row, col, dep];
          }
        }
      }
      return output;
    }

    public static T[] Resize<T>(this T[,] array, bool colFirst = true)
    {
      var rowN = array.GetLength(0);
      var colN = array.GetLength(1);
      var output = new T[rowN * colN];
      var idx = 0;
      if (colFirst)
      {
        for (var row = 0; row < rowN; row++)
          for (var col = 0; col < colN; col++)
            output[idx++] = array[row, col];
      }
      else
      {
        for (var col = 0; col < colN; col++)
          for (var row = 0; row < rowN; row++)
            output[idx++] = array[row, col];
      }
      return output;
    }
    public static T[] Resize<T>(this T[,,] array)
    {
      var rowN = array.GetLength(0);
      var colN = array.GetLength(1);
      var depN = array.GetLength(2);
      var output = new T[rowN * colN * depN];
      var idx = 0;
      for (var row = 0; row < rowN; row++)
        for (var col = 0; col < colN; col++)
          for (var dep = 0; dep < depN; dep++)
            output[idx++] = array[row, col, dep];
      return output;
    }
    public static T[,] Resize<T>(this T[,] array, int rowN, int colN)
    {
      var vector = array.Resize();
      var output = new T[rowN, colN];
      var idx = 0;
      for (var row = 0; row < rowN; row++)
        for (var col = 0; col < colN; col++)
          output[row, col] = vector[idx++];
      return output;
    }

    public static bool IsEqual<T>(this T[] array1, T[] array2)
    {
      var length = array1.GetLength(0);
      if (length != array2.GetLength(0))
        return false;
      for (var i = 0; i < length; i++)
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
      for (var row = 0; row < rowN; row++)
        for (var col = 0; col < colN; col++)
          if (!array1[row, col].Equals(array2[row, col]))
            return false;
      return true;

    }

    public static T[] GetCol<T>(this T[,] array, int col)
    {
      return Enumerable.Range(0, array.GetLength(0))
              .Select(x => array[x, col])
              .ToArray();
    }
    public static T[] GetRow<T>(this T[,] array, int row)
    {
      return Enumerable.Range(0, array.GetLength(1))
              .Select(x => array[row, x])
              .ToArray();
    }

    public static T[] GetY<T>(this T[,] array, int y) 
      => array.GetCol(y);
    public static T[] GetX<T>(this T[,] array, int x)
      => array.GetRow(x);

    public static List<T> GetRange<T>(this T[] array, int index, int count)
      => array.ToList().GetRange(index, count);
    /// <summary>
    /// Return a list of elements in <paramref name="array"/> starting from <paramref name="index"/> to end
    /// </summary>
    public static List<T> GetRange<T>(this T[] array, int index)
      => array.GetRange(index, array.Length - index);

    public static void Clear<T>(this T[,] array)
    {
      var L1 = array.GetLength(0);
      var L2 = array.GetLength(1);
      for (var i = 0; i < L1; i++)
      {
        for (var j = 0; j < L2; j++)
        {
          array[i, j] = default;
        }
      }
    }

  }
  
}