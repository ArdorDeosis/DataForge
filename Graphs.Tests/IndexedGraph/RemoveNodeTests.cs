using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

[TestFixture]
public class RemoveNodeTests
{
  [Test]
  public void RemoveNode_NodeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.True);
  }

  [Test]
  public void RemoveNode_IndexInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(0, 0);

    // ASSERT
    Assert.That(graph.RemoveNode(0), Is.True);
  }

  [Test]
  public void RemoveNode_NodeInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ACT
    graph.RemoveNode(0);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void RemoveNode_NodeInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);

    // ACT
    graph.RemoveNode(0);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void RemoveNode_NodeInGraph_GraphDoesNotContainIndex()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices.Contains(index), Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_GraphDoesNotContainIndex()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    graph.AddNode(index, 0);

    // ACT
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices.Contains(index), Is.False);
  }

  [Test]
  public void RemoveNode_InvalidNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var nodeInOtherGraph = new IndexedGraph<int, int, int>().AddNode(0, 0);
    const int removedIndex = 0xC0FFEE;
    var removedNode = graph.AddNode(removedIndex, 0);
    graph.RemoveNode(removedIndex);

    // ASSERT
    Assert.That(graph.RemoveNode(nodeInOtherGraph), Is.False);
    Assert.That(graph.RemoveNode(removedNode), Is.False);
  }

  [Test]
  public void RemoveNode_InvalidIndex_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.RemoveNode(0), Is.False);
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };

    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    graph.AddNode(indices[2], 0);
    var edge1 = graph.AddEdge(indices[0], indices[1], 0);
    var edge2 = graph.AddEdge(indices[1], indices[2], 0);

    // ACT
    graph.RemoveNode(indices[1]);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };

    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    graph.AddNode(indices[2], 0);
    var edge1 = graph.AddEdge(indices[0], indices[1], 0);
    var edge2 = graph.AddEdge(indices[1], indices[2], 0);

    // ACT
    graph.RemoveNode(indices[1]);

    // ASSERT
    Assert.That(edge1.IsValid, Is.False);
    Assert.That(edge2.IsValid, Is.False);
  }

  [Test]
  public void RemoveNodesWhere_ExpectedNodesAreRemoved()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node1 = graph.AddNode(-1, -1);
    var node2 = graph.AddNode(1, 1);

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node1));
    Assert.That(graph.Nodes, Does.Not.Contain(node2));
  }
}