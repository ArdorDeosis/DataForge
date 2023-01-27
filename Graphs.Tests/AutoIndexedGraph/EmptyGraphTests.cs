using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class EmptyGraphTests
{
  [Test]
  public void EmptyGraph_HasNoNodes()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Nodes, Is.Empty);
  }

  [Test]
  public void EmptyGraph_HasNoEdges()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Edges, Is.Empty);
  }

  [Test]
  public void EmptyGraph_OrderIsZero()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.Order, Is.Zero);
  }

  [Test]
  public void EmptyGraph_SizeIsZero()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.Size, Is.Zero);
  }
}