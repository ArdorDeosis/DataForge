using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
// ReSharper disable once InconsistentNaming
public abstract class IIndexedGraphTests<TGraph> where TGraph : IIndexedGraph<int, int, int>
{
  private const int UnrelatedIndex = 359194993;

  /// <summary> An empty graph. </summary>
  protected abstract TGraph EmptyGraph { get; }

  /// <summary> A graph and a node expected to be in the graph. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> expectedNode) GraphWithNode { get; }

  /// <summary> A graph with two nodes. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> node1, IndexedNode<int, int, int> node2)
    GraphWithTwoNodes { get; }

  /// <summary> A graph and an edge expected to be in the graph. </summary>
  protected abstract (TGraph graph, IndexedEdge<int, int, int> expectedEdge) GraphWithEdge { get; }

  /// <summary> An empty graph and a node that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> removedNode) EmptyGraphWithRemovedNode { get; }

  /// <summary> An empty graph and an edge that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, IndexedEdge<int, int, int> removedEdge) EmptyGraphWithRemovedEdge { get; }

  /// <summary> A single node inside a graph not obtainable by other setup methods. </summary>
  protected abstract IndexedNode<int, int, int> NodeFromOtherGraph { get; }

  /// <summary> A single edge inside a graph not obtainable by other setup methods. </summary>
  protected abstract IndexedEdge<int, int, int> EdgeFromOtherGraph { get; }

  [Test]
  public void RemoveNode_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.True);
  }

  [Test]
  public void RemoveNode_ExistingNode_NodeIsRemoved()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes, Does.Not.Contain(node));
  }

  [Test]
  public void RemoveNode_ExistingNode_NodeIsInvalidated()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(node, Is.Invalid);
  }

  [Test]
  public void RemoveNode_UnexpectedNode_ReturnsFalse()
  {
    // ARRANGE
    var (graph, removedNode) = EmptyGraphWithRemovedNode;

    // ASSERT
    Assert.That(graph.RemoveNode(removedNode), Is.False);
    Assert.That(graph.RemoveNode(NodeFromOtherGraph), Is.False);
  }

  [Test]
  public void RemoveNodeByIndex_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ASSERT
    Assert.That(graph.RemoveNode(node.Index), Is.True);
  }

  [Test]
  public void RemoveNodeByIndexWithOutParameter_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ASSERT
    Assert.That(graph.RemoveNode(node.Index, out _), Is.True);
  }

  [Test]
  public void RemoveNodeByIndex_ExistingNode_OutputsNode()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ACT
    graph.RemoveNode(expectedNode.Index, out var retrievedNode);

    // ASSERT
    Assert.That(retrievedNode, Is.EqualTo(expectedNode));
  }

  [Test]
  public void RemoveNodeByIndex_ExistingNode_NodeIsRemoved()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;
    var index = node.Index;

    // ACT
    graph.RemoveNode(node.Index);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices, Does.Not.Contain(index));
  }

  [Test]
  public void RemoveNodeByIndexWithOutParameter_ExistingNode_NodeIsRemoved()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;
    var index = node.Index;

    // ACT
    graph.RemoveNode(node.Index, out _);

    // ASSERT
    Assert.That(graph.Contains(index), Is.False);
    Assert.That(graph.Indices, Does.Not.Contain(index));
  }

  [Test]
  public void RemoveNodeByIndex_ExistingNode_AdjacentEdgesAreRemoved()
  {
    // ARRANGE
    var (graph, edge) = GraphWithEdge;

    // ACT
    graph.RemoveNode(edge.Origin.Index);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge));
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveNodeByIndexWithOutParameter_ExistingNode_AdjacentEdgesAreRemoved()
  {
    // ARRANGE
    var (graph, edge) = GraphWithEdge;

    // ACT
    graph.RemoveNode(edge.Origin.Index, out _);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge));
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveNodeByIndex_UnexpectedNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ASSERT
    Assert.That(graph.RemoveNode(UnrelatedIndex), Is.False);
    Assert.That(graph.RemoveNode(UnrelatedIndex, out _), Is.False);
  }


  [Test]
  public void AddEdge_ValidIndices_AddedEdgeHasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var (graph, node1, node2) = GraphWithTwoNodes;

    // ACT
    var edge = graph.AddEdge(node1.Index, node2.Index, data);

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_ValidIndices_GraphContainsEdge()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var (graph, node1, node2) = GraphWithTwoNodes;

    // ACT
    var edge = graph.AddEdge(node1.Index, node2.Index, data);

    // ASSERT
    Assert.That(graph.Edges, Does.Contain(edge));
  }

  [Test]
  public void AddEdge_UnexpectedIndices_ThrowsArgumentException()
  {
    // ARRANGE
    var (graph, node, _) = GraphWithTwoNodes;

    // ASSERT
    Assert.That(() => graph.AddEdge(node.Index, UnrelatedIndex, default), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(UnrelatedIndex, node.Index, default), Throws.ArgumentException);
  }

  [Test]
  public void TryAddEdge_ValidNodes_ReturnsTrue()
  {
    // ARRANGE
    var (graph, node1, node2) = GraphWithTwoNodes;

    // ASSERT
    Assert.That(
      graph.TryAddEdge(node1.Index, node2.Index, default, out _),
      Is.True);
  }

  [Test]
  public void TryAddEdge_AddedEdge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var (graph, node1, node2) = GraphWithTwoNodes;

    // ACT
    graph.TryAddEdge(node1.Index, node2.Index, data, out var edge);

    // ASSERT
    Assert.That(edge, Is.Not.Null);
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddEdge_GraphContainsEdge()
  {
    // ARRANGE
    var (graph, node1, node2) = GraphWithTwoNodes;

    // ACT
    graph.TryAddEdge(node1.Index, node2.Index, default, out var edge);

    // ASSERT
    Assert.That(graph.Edges, Does.Contain(edge));
  }

  [Test]
  public void TryAddEdge_UnexpectedNode_ReturnsFalse()
  {
    // ARRANGE
    var (graph, node, _) = GraphWithTwoNodes;

    // ASSERT
    Assert.That(graph.TryAddEdge(node.Index, UnrelatedIndex, default, out _), Is.False);
    Assert.That(graph.TryAddEdge(UnrelatedIndex, node.Index, default, out _), Is.False);
  }

  [Test]
  public void RemoveEdge_ExistingEdge_ReturnsTrue()
  {
    // ARRANGE
    var (graph, expectedEdge) = GraphWithEdge;

    // ASSERT
    Assert.That(graph.RemoveEdge(expectedEdge), Is.True);
  }

  [Test]
  public void RemoveEdge_ExistingEdge_EdgeIsRemoved()
  {
    // ARRANGE
    var (graph, expectedEdge) = GraphWithEdge;

    // ACT
    graph.RemoveEdge(expectedEdge);

    // ASSERT
    Assert.That(graph.Contains(expectedEdge), Is.False);
    Assert.That(graph.Edges, Does.Not.Contain(expectedEdge));
  }

  [Test]
  public void RemoveEdge_ExistingEdge_EdgeIsInvalidated()
  {
    // ARRANGE
    var (graph, expectedEdge) = GraphWithEdge;

    // ACT
    graph.RemoveEdge(expectedEdge);

    // ASSERT
    Assert.That(expectedEdge, Is.Invalid);
  }

  [Test]
  public void RemoveEdge_UnexpectedEdge_ReturnsFalse()
  {
    // ARRANGE
    var (graph, removedEdge) = EmptyGraphWithRemovedEdge;

    // ASSERT
    Assert.That(graph.RemoveEdge(removedEdge), Is.False);
    Assert.That(graph.RemoveEdge(EdgeFromOtherGraph), Is.False);
  }
}