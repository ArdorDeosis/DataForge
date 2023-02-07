using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
// ReSharper disable once InconsistentNaming
public abstract class IReadOnlyGraphTests<TGraph> where TGraph : IReadOnlyGraph<int, int>
{
  /// <summary> An empty graph. </summary>
  protected abstract TGraph GetEmptyGraph { get; }

  /// <summary> A graph and the nodes expected to be in the graph. </summary>
  protected abstract (TGraph graph, INode<int, int>[] expectedNodes) GraphWithNodes { get; }

  /// <summary> A graph and the edges expected to be in the graph. </summary>
  protected abstract (TGraph graph, IEdge<int, int>[] expectedEdges) GraphWithEdges { get; }

  /// <summary> An empty graph and a node that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode { get; }

  /// <summary> A graph without edges and an edge that was in the graph but has been removed. </summary>
  protected abstract (TGraph graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge { get; }

  /// <summary> A single node inside a graph not obtainable by other setup methods. </summary>
  protected abstract INode<int, int> NodeFromOtherGraph { get; }

  /// <summary> A single edge inside a graph not obtainable by other setup methods. </summary>
  protected abstract IEdge<int, int> EdgeFromOtherGraph { get; }

  [Test]
  public void Nodes_EmptyGraph_IsEmpty()
  {
    // ARRANGE
    var graph = GetEmptyGraph;

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
  }

  [Test]
  public void Nodes_ContainsExpectedNodes()
  {
    // ARRANGE
    var (graph, expectedNodes) = GraphWithNodes;

    // ASSERT
    Assert.That(graph.Nodes, Is.EquivalentTo(expectedNodes));
  }

  [Test]
  public void Nodes_DoesNotContainUnexpectedNodes()
  {
    // ARRANGE
    var (graph, removedNode) = EmptyGraphWithRemovedNode;
    var nodeInOtherGraph = NodeFromOtherGraph;

    // ASSERT
    Assert.That(graph.Nodes, Does.Not.Contain(removedNode));
    Assert.That(graph.Nodes, Does.Not.Contain(nodeInOtherGraph));
  }

  [Test]
  public void Edges_EmptyGraph_IsEmpty()
  {
    // ARRANGE
    var graph = GetEmptyGraph;

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Edges_ContainsExpectedNodes()
  {
    // ARRANGE
    var (graph, expectedEdges) = GraphWithEdges;

    // ASSERT
    Assert.That(graph.Edges, Is.EquivalentTo(expectedEdges));
  }

  [Test]
  public void Edges_DoesNotContainUnexpectedNodes()
  {
    // ARRANGE
    var (graph, removedEdge) = EmptyGraphWithRemovedEdge;
    var edgeInOtherGraph = EdgeFromOtherGraph;

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(removedEdge));
    Assert.That(graph.Edges, Does.Not.Contain(edgeInOtherGraph));
  }

  [Test]
  public void ContainsNode_ContainedNode_True()
  {
    // ARRANGE
    var (graph, expectedNodes) = GraphWithNodes;

    // ASSERT
    foreach (var expectedNode in expectedNodes)
      Assert.That(graph.Contains(expectedNode), Is.True);
  }

  [Test]
  public void ContainsNode_NotContainedNode_False()
  {
    // ARRANGE
    var (graph, removedNode) = EmptyGraphWithRemovedNode;
    var nodeInOtherGraph = NodeFromOtherGraph;

    // ASSERT
    Assert.That(graph.Contains(removedNode), Is.False);
    Assert.That(graph.Contains(nodeInOtherGraph), Is.False);
  }

  [Test]
  public void ContainsEdge_ContainedEdge_True()
  {
    // ARRANGE
    var (graph, expectedEdges) = GraphWithEdges;

    // ASSERT
    foreach (var expectedEdge in expectedEdges)
      Assert.That(graph.Contains(expectedEdge), Is.True);
  }

  [Test]
  public void ContainsEdge_NotContainedEdge_False()
  {
    // ARRANGE
    var (graph, removedEdge) = EmptyGraphWithRemovedEdge;
    var edgeInOtherGraph = EdgeFromOtherGraph;

    // ASSERT
    Assert.That(graph.Contains(removedEdge), Is.False);
    Assert.That(graph.Contains(edgeInOtherGraph), Is.False);
  }

  [Test]
  public void Order_EmptyGraph_IsZero()
  {
    // ARRANGE
    var graph = GetEmptyGraph;

    // ASSERT
    Assert.That(graph.Order, Is.Zero);
  }

  [Test]
  public void Order_GraphWithNodes_IsNodeCount()
  {
    // ARRANGE
    var (graph, expectedNodes) = GraphWithNodes;

    // ASSERT
    Assert.That(graph.Order, Is.EqualTo(expectedNodes.Length));
  }

  [Test]
  public void Order_GraphWithRemovedNode_IsZero()
  {
    // ARRANGE
    var (graph, expectedNodes) = EmptyGraphWithRemovedNode;

    // ASSERT
    Assert.That(graph.Order, Is.Zero);
  }

  [Test]
  public void Size_EmptyGraph_IsZero()
  {
    // ARRANGE
    var graph = GetEmptyGraph;

    // ASSERT
    Assert.That(graph.Size, Is.Zero);
  }

  [Test]
  public void Size_GraphWithEdges_IsEdgeCount()
  {
    // ARRANGE
    var (graph, expectedEdges) = GraphWithEdges;

    // ASSERT
    Assert.That(graph.Size, Is.EqualTo(expectedEdges.Length));
  }

  [Test]
  public void Size_RemovedEdge_IsZero()
  {
    // ARRANGE
    var (graph, _) = EmptyGraphWithRemovedEdge;

    // ASSERT
    Assert.That(graph.Size, Is.Zero);
  }
}