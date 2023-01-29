using DataForge.Graphs;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.IndexedGraph;

internal class EmptyGraphTests
{
  [Test]
  public void EmptyGraph_HasNoNodes()
  {
    // ARRANGE
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Nodes, Is.Empty);
  }

  [Test]
  public void EmptyGraph_HasNoEdges()
  {
    // ARRANGE
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Edges, Is.Empty);
  }

  [Test]
  public void EmptyGraph_OrderIsZero()
  {
    // ARRANGE
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.Order, Is.Zero);
  }

  [Test]
  public void EmptyGraph_SizeIsZero()
  {
    // ARRANGE
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.Size, Is.Zero);
  }
}