using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

internal class RemoveEdgeTests
{
  [Test]
  public void RemoveEdge_EdgeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.True);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_EdgeIsInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_GraphDoesNotContainEdge()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
    Assert.That(graph.Edges.Contains(edge), Is.False);
  }

  [Test]
  public void RemoveEdge_InvalidEdge_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var otherGraph = new Graph<int, int>();
    var edgeInOtherGraph = otherGraph.AddEdge(otherGraph.AddNode(0), otherGraph.AddNode(0), 0);
    var removedEdge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);
    graph.RemoveEdge(removedEdge);

    // ASSERT
    Assert.That(graph.RemoveEdge(edgeInOtherGraph), Is.False);
    Assert.That(graph.RemoveEdge(removedEdge), Is.False);
  }

  [Test]
  public void RemoveEdgesWhere_ExpectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge1 = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), -1);
    var edge2 = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Contains(edge1), Is.True);
    Assert.That(graph.Edges, Does.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void RemoveEdgesWhere_RemovedEdgesAreInvalidated()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveEdgesWhere_ReturnedNumber_IsCorrect()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(0), graph.AddNode(0), -1);
    graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 1);

    // ACT
    var removedEdges = graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(removedEdges, Is.EqualTo(1));
  }
}