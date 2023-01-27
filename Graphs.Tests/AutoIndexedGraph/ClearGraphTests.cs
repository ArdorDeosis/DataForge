using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class ClearGraphTests
{
  [Test]
  public void Clear_RemovesNodes()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    graph.AddNode(0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
  }

  [Test]
  public void Clear_RemovesEdges()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    graph.AddEdge(
      graph.AddNode(default).Index,
      graph.AddNode(default).Index,
      default);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Clear_RemovedNodesAreInvalid()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var node = graph.AddNode(default);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void Clear_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var edge = graph.AddEdge(
      graph.AddNode(default).Index,
      graph.AddNode(default).Index,
      default);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edge.IsValid, Is.False);
  }
}