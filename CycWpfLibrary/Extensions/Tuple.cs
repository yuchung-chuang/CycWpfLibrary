namespace CycWpfLibrary
{
  public static class TupleExtensions
  {
    public static (double, double) Times(this (double, double) tuple, double scale)
    {
      return (tuple.Item1 * scale, tuple.Item2 * scale);
    }

    public static (double, double) Add(this (double, double) tuple1, (double, double) tuple2)
    {
      return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
    }

    public static (double, double) Minus(this (double, double) tuple1, (double, double) tuple2)
    {
      return (tuple1.Item1 - tuple2.Item1, tuple1.Item2 - tuple2.Item2);
    }
  }
}
