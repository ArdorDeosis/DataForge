using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs.Tests;

/// <remarks>Generates collisions for even numbers and their directly following odd numbers.</remarks>
internal class TestEqualityComparer : IEqualityComparer<int>
{
  public bool Equals(int x, int y) => x / 2 == y / 2;
  public int GetHashCode(int obj) => obj / 2;

  internal static (int index1, int index2) EquivalentIndexPair => (0, 1);

  [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
  internal static int[] EquivalentIndexArray => EquivalentIndexPair.ToArray();
}