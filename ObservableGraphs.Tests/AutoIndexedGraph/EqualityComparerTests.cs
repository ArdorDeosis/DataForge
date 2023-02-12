using DataForge.Graphs.Tests;
using NUnit.Framework;

namespace DataForge.Graphs.Observable.Tests.AutoIndexedGraph;

internal class EqualityComparerTests
{
  [Test]
  public void EqualityComparerInstance_IsUsed()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(
      new IncrementalIndexProvider<int, int>(0),
      new TestEqualityComparer());
    graph.AddNode(default);

    // ASSERT
    Assert.That((TestDelegate)(() => graph.AddNode(default)), Throws.InvalidOperationException);
  }

  [Test]
  public void EqualityComparerFactoryMethod_IsUsed()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(
      new IncrementalIndexProvider<int, int>(0),
      () => new TestEqualityComparer());
    graph.AddNode(default);

    // ASSERT
    Assert.That((TestDelegate)(() => graph.AddNode(default)), Throws.InvalidOperationException);
  }
}