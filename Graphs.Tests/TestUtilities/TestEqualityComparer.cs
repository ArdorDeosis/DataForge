using System.Collections.Generic;

namespace DataForge.Graphs.Tests;

/// <remarks>Generates collisions for even numbers and their directly following odd numbers.</remarks>
public class TestEqualityComparer : IEqualityComparer<int>
{
  public static (int index1, int index2) EquivalentIndexPair => (0, 1);

  public bool Equals(int x, int y) => x / 2 == y / 2;

  public int GetHashCode(int obj) => obj / 2;
}