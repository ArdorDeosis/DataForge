namespace Utilities;

public static class EnumerableExtensions
{
  public static IEnumerable<(T1, T2)> PairWith<T1, T2>(this IEnumerable<T1> collection1, IEnumerable<T2> collection2) =>
    from item1 in collection1 from item2 in collection2 select (item1, item2);
}