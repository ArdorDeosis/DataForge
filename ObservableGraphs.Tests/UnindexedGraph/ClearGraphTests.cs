using NUnit.Framework;
using Is = DataForge.Graphs.Tests.Is;

namespace DataForge.ObservableGraphs.Tests.UnindexedGraph;

internal class ClearGraphTests
{
  [Test]
  public void Clear_RemovesNodes()
  {
    // ARRANGE
    var graph = new ObservableGraph<int, int>();
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
    var graph = new ObservableGraph<int, int>();
    graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Clear_RemovedNodesAreInvalid()
  {
    // ARRANGE
    var graph = new ObservableGraph<int, int>();
    var node = graph.AddNode(0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void Clear_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new ObservableGraph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }
}