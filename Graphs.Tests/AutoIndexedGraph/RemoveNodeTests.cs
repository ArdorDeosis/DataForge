using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class RemoveNodeTests
{
  private static (AutoIndexedGraph<int, int, int> graph, int index, IndexedNode<int, int, int> node) SetupSingleNode()
  {
    const int index = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);
    return (graph, index, node);
  }

  private static (AutoIndexedGraph<int, int, int> graph,
    IndexedNode<int, int, int> invalidNode,
    IndexedNode<int, int, int> removedNode) SetupInvalidNodes()
  {
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var otherGraph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var removedNode = graph.AddNode(default);
    graph.RemoveNode(removedNode);
    var invalidNode = otherGraph.AddNode(default);
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
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void IGraphRemoveNode_NodeInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var (graph, _, node) = SetupSingleNode();

    // ACT
    (graph as IGraph<int, int>).RemoveNode(node);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void RemoveNode_IndexInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var (graph, index, node) = SetupSingleNode();

    // ACT
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
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
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.RemoveNode(0), Is.False);
    Assert.That(graph.RemoveNode(0, out _), Is.False);
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    var middleNode = graph.AddNode(default);
    var edge1 = graph.AddEdge(graph.AddNode(default).Index, middleNode.Index, 0);
    var edge2 = graph.AddEdge(middleNode.Index, graph.AddNode(default).Index, 0);

    // ACT
    graph.RemoveNode(middleNode);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    var middleNode = graph.AddNode(default);
    var edge1 = graph.AddEdge(graph.AddNode(default).Index, middleNode.Index, 0);
    var edge2 = graph.AddEdge(middleNode.Index, graph.AddNode(default).Index, 0);

    // ACT
    graph.RemoveNode(middleNode);

    // ASSERT
    Assert.That(edge1.IsValid, Is.False);
    Assert.That(edge2.IsValid, Is.False);
  }
  
  [Test]
  public void RemoveNodesWhere_ExpectedNodesAreRemoved()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var node1 = graph.AddNode(-1);
    var node2 = graph.AddNode(1);

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
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var node = graph.AddNode(1);

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }
}