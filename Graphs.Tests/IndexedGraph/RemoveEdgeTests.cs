using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

[TestFixture]
public class RemoveEdgeTests
{
  [Test]
  public void RemoveEdge_EdgeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };

    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    var edge = graph.AddEdge(indices[0], indices[1], 0);

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.True);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_EdgeIsInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };

    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    var edge = graph.AddEdge(indices[0], indices[1], 0);

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(edge.IsValid, Is.False);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_GraphDoesNotContainEdge()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };

    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    var edge = graph.AddEdge(indices[0], indices[1], 0);

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
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    var otherGraph = new IndexedGraph<int, int, int>();
    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    otherGraph.AddNode(indices[0], 0);
    otherGraph.AddNode(indices[1], 0);
    var edgeInOtherGraph = otherGraph.AddEdge(indices[0], indices[1], 0);
    var removedEdge = graph.AddEdge(indices[0], indices[1], 0);
    graph.RemoveEdge(removedEdge);

    // ASSERT
    Assert.That(graph.RemoveEdge(edgeInOtherGraph), Is.False);
    Assert.That(graph.RemoveEdge(removedEdge), Is.False);
  }

  [Test]
  public void RemoveEdgesWhere_ExpectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    graph.AddNode(indices[0], indices[0]);
    graph.AddNode(indices[1], indices[1]);
    var edge1 = graph.AddEdge(indices[0], indices[1], -1);
    var edge2 = graph.AddEdge(indices[0], indices[1], 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Contains(edge1), Is.True);
    Assert.That(graph.Edges, Does.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }
}