using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

internal class RemoveNodeTests
{
  private (IndexedGraph<int, int, int> graph, int index, IndexedNode<int, int, int> node) SetupSingleNode()
  {
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);
    return (graph, index, node);
  }

  private (IndexedGraph<int, int, int> graph,
    IndexedNode<int, int, int> invalidNode,
    IndexedNode<int, int, int> removedNode) SetupInvalidNodes()
  {
    var graph = new IndexedGraph<int, int, int>();
    var otherGraph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    const int removedIndex = 0xBEEF;
    const int invalidIndex = 0xF00D;
    var removedNode = graph.AddNode(removedIndex, 0);
    graph.RemoveNode(removedNode);
    var invalidNode = otherGraph.AddNode(index, 0);
    return (graph, invalidNode, removedNode);
  }

  [Test]
  public void RemoveNode_NodeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.True);
  }

  [Test]
  public void IGraphRemoveNode_NodeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ASSERT
    Assert.That((graph as IGraph<int, int>).RemoveNode(node), Is.True);
  }

  [Test]
  public void RemoveNode_IndexInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, index, _) = SetupSingleNode();

    // ASSERT
    Assert.That(graph.RemoveNode(index), Is.True);
  }

  [Test]
  public void RemoveNodeOutNode_IndexInGraph_ReturnsTrue()
  {
    // ARRANGE
    var (graph, index, _) = SetupSingleNode();

    // ASSERT
    Assert.That(graph.RemoveNode(index, out _), Is.True);
  }

  [Test]
  public void RemoveNodeOutNode_IndexInGraph_OutputsNode()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(index, out var retrievedNode);

    // ASSERT
    Assert.That(retrievedNode, Is.EqualTo(node));
  }

  [Test]
  public void RemoveNode_NodeInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void IGraphRemoveNode_NodeInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ACT
    (graph as IGraph<int, int>).RemoveNode(node);

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void RemoveNode_IndexInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void RemoveNode_NodeInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void IGraphRemoveNode_NodeInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ACT
    (graph as IGraph<int, int>).RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void RemoveNode_NodeInGraph_GraphDoesNotContainIndex()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices.Contains(index), Is.False);
  }

  [Test]
  public void IGraphRemoveNode_NodeInGraph_GraphDoesNotContainIndex()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    (graph as IGraph<int, int>).RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices.Contains(index), Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_GraphDoesNotContainIndex()
  {
    // ARRANGE
    var (graph, index, _) = SetupSingleNode();

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
    var (graph, invalidNode, removedNode) = SetupInvalidNodes();

    // ASSERT
    Assert.That(graph.RemoveNode(invalidNode), Is.False);
    Assert.That(graph.RemoveNode(removedNode), Is.False);
  }

  [Test]
  public void IGraphRemoveNode_InvalidNode_ReturnsFalse()
  {
    // ARRANGE
    var (graph, invalidNode, removedNode) = SetupInvalidNodes();

    // ASSERT
    Assert.That((graph as IGraph<int, int>).RemoveNode(invalidNode), Is.False);
    Assert.That((graph as IGraph<int, int>).RemoveNode(removedNode), Is.False);
  }

  [Test]
  public void RemoveNode_InvalidIndex_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.RemoveNode(0), Is.False);
    Assert.That(graph.RemoveNode(0, out _), Is.False);
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
    Assert.That(edge1, Is.Invalid);
    Assert.That(edge2, Is.Invalid);
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

  [Test]
  public void RemoveNodesWhere_RemovedNodeIsInvalid()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(1, 1);

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void RemoveNodesWhere_AdjacentEdgesAreRemovedAndInvalidated()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index1 = -1;
    const int index2 = 1;
    graph.AddNode(index1, index1);
    graph.AddNode(index2, index2);
    var edge1 = graph.AddEdge(index2, index1, default);
    var edge2 = graph.AddEdge(index1, index2, default);

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
    Assert.That(edge1, Is.Invalid);
    Assert.That(edge2, Is.Invalid);
  }

  [Test]
  public void RemoveNodesWhere_ReturnedNumber_IsCorrect()
  {
    // TODO
  }
}