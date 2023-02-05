using DataForge.Graphs;
using NUnit.Framework;
using Is = DataForge.Graphs.Tests.Is;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

internal class ClearGraphTests
{
  [Test]
  public void Clear_RemovesNodes()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
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
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
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
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var node = graph.AddNode(default);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void Clear_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var edge = graph.AddEdge(
      graph.AddNode(default).Index,
      graph.AddNode(default).Index,
      default);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }
}