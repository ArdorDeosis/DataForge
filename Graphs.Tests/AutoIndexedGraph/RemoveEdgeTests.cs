using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class RemoveEdgeTests
{
  private static (AutoIndexedGraph<int, int, int> graph,
    IndexedEdge<int, int, int> edge,
    IndexedEdge<int, int, int> edgeInOtherGraph,
    IndexedEdge<int, int, int> removedEdge) Setup()
  {
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var otherGraph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var index1 = graph.AddNode(default).Index;
    var index2 = graph.AddNode(default).Index;
    var edgeInOtherGraph = otherGraph.AddEdge(
      otherGraph.AddNode(default).Index,
      otherGraph.AddNode(default).Index,
      default);
    var edge = graph.AddEdge(index1, index2, 0);
    var removedEdge = graph.AddEdge(index1, index2, 0);
    graph.RemoveEdge(removedEdge);

    return (graph, edge, edgeInOtherGraph, removedEdge);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.True);
  }

  [Test]
  public void IGraphRemoveEdge_EdgeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ASSERT
    Assert.That((graph as IGraph<int, int>).RemoveEdge(edge), Is.True);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_EdgeIsInvalid()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void IGraphRemoveEdge_EdgeInGraph_EdgeIsInvalid()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ACT
    (graph as IGraph<int, int>).RemoveEdge(edge);

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_GraphDoesNotContainEdge()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
    Assert.That(graph.Edges.Contains(edge), Is.False);
  }

  [Test]
  public void IGraphRemoveEdge_EdgeInGraph_GraphDoesNotContainEdge()
  {
    // ARRANGE
    var (graph, edge, _, _) = Setup();

    // ACT
    (graph as IGraph<int, int>).RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
    Assert.That(graph.Edges.Contains(edge), Is.False);
  }

  [Test]
  public void RemoveEdge_InvalidEdge_ReturnsFalse()
  {
    // ARRANGE
    var (graph, _, edgeInOtherGraph, removedEdge) = Setup();

    // ASSERT
    Assert.That(graph.RemoveEdge(edgeInOtherGraph), Is.False);
    Assert.That(graph.RemoveEdge(removedEdge), Is.False);
  }

  [Test]
  public void IGraphRemoveEdge_InvalidEdge_ReturnsFalse()
  {
    // ARRANGE
    var (graph, _, edgeInOtherGraph, removedEdge) = Setup();

    // ASSERT
    Assert.That((graph as IGraph<int, int>).RemoveEdge(edgeInOtherGraph), Is.False);
    Assert.That((graph as IGraph<int, int>).RemoveEdge(removedEdge), Is.False);
  }

  [Test]
  public void RemoveEdgesWhere_ExpectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var index1 = graph.AddNode(default).Index;
    var index2 = graph.AddNode(default).Index;
    var edge1 = graph.AddEdge(index1, index2, -1);
    var edge2 = graph.AddEdge(index1, index2, 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Contains(edge1), Is.True);
    Assert.That(graph.Edges, Does.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void RemoveEdgesWhere_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }


  [Test]
  public void RemoveEdgesWhere_ReturnedNumber_IsCorrect()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, -1);
    graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, 1);

    // ACT
    var removedEdges = graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(removedEdges, Is.EqualTo(1));
  }
}