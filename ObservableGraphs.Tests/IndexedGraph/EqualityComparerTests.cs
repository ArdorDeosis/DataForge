using DataForge.Graphs.Tests;
using NUnit.Framework;

namespace DataForge.Graphs.Observable.Tests.IndexedGraph;

internal class EqualityComparerTests
{
  [Test]
  public void EqualityComparerInstance_IsUsed()
  {
    // ARRANGE
    var (index, equivalentIndex) = TestEqualityComparer.EquivalentIndexPair;
    var graph = new ObservableIndexedGraph<int, int, int>(new TestEqualityComparer());
    graph.AddNode(index, 0);

    // ASSERT
    Assert.That((TestDelegate)(() => graph.AddNode(equivalentIndex, 0)), Throws.InvalidOperationException);
  }

  [Test]
  public void EqualityComparerFactoryMethod_IsUsed()
  {
    // ARRANGE
    var (index, equivalentIndex) = TestEqualityComparer.EquivalentIndexPair;
    var graph = new ObservableIndexedGraph<int, int, int>(() => new TestEqualityComparer());
    graph.AddNode(index, 0);

    // ASSERT
    Assert.That((TestDelegate)(() => graph.AddNode(equivalentIndex, 0)), Throws.InvalidOperationException);
  }
}