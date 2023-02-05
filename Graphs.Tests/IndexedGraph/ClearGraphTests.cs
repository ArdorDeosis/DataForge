using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

internal class ClearGraphTests
{
  [Test]
  public void Clear_RemovesNodes()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(0, 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
  }

  [Test]
  public void Clear_RemovesEdges()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    graph.AddNode(indices[0], indices[0]);
    graph.AddNode(indices[1], indices[1]);
    graph.AddEdge(indices[0], indices[1], 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Clear_RemovedNodesAreInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void Clear_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    graph.AddNode(indices[0], indices[0]);
    graph.AddNode(indices[1], indices[1]);
    var edge = graph.AddEdge(indices[0], indices[1], 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }
}