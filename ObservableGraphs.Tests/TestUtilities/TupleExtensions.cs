namespace DataForge.ObservableGraphs.Tests.TestUtilities;

internal static class TupleExtensions
{
  internal static T[] ToArray<T>(this (T, T) tuple) => new[] { tuple.Item1, tuple.Item2 };
}