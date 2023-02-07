using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
// ReSharper disable once InconsistentNaming
public abstract class IGraphTests<TGraph> where TGraph : IGraph<int, int>
{
  /// <summary> A graph and a node expected to be in the graph. </summary>
  protected abstract (TGraph graph, INode<int, int> expectedNode) GraphWithNode { get; }

  /// <summary> A graph and nodes expected to be in the graph. </summary>
  protected abstract (TGraph graph, IReadOnlyCollection<INode<int, int>> expectedNodes) GraphWithNodes { get; }

  /// <summary> A graph containing nodes with the provided data. </summary>
  protected abstract (TGraph graph, IReadOnlyCollection<INode<int, int>> expectedNodes) GraphWithNodesWithData(
    IReadOnlyCollection<int> data);

  /// <summary> A graph and an edge expected to be in the graph. </summary>
  protected abstract (TGraph graph, IEdge<int, int> expectedEdge) GraphWithEdge { get; }

  /// <summary> A graph and edges expected to be in the graph. </summary>
  protected abstract (TGraph graph, IReadOnlyCollection<IEdge<int, int>> expectedEdges) GraphWithEdges { get; }

  /// <summary> A graph containing edges with the provided data. </summary>
  protected abstract (TGraph graph, IReadOnlyCollection<IEdge<int, int>> expectedEdges) GraphWithEdgesWithData(
    IReadOnlyCollection<int> data);

  /// <summary> An empty graph and a node that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode { get; }

  /// <summary> An empty graph and an edge that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge { get; }

  /// <summary> A single node inside a graph not obtainable by other setup methods. </summary>
  protected abstract INode<int, int> NodeFromOtherGraph { get; }

  /// <summary> A single edge inside a graph not obtainable by other setup methods. </summary>
  protected abstract IEdge<int, int> EdgeFromOtherGraph { get; }

  [Test]
  public void RemoveNode_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ASSERT
    Assert.That(graph.RemoveNode(expectedNode), Is.True);
  }

  [Test]
  public void RemoveNode_ExistingNode_NodeIsRemoved()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ACT
    graph.RemoveNode(expectedNode);

    // ASSERT
    Assert.That(graph.Contains(expectedNode), Is.False);
    Assert.That(graph.Nodes, Does.Not.Contain(expectedNode));
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
  public void RemoveNode_AdjacentEdgeIsRemovedAndInvalidated()
  {
    // ARRANGE
    var (graph, edge) = GraphWithEdge;

    // ACT
    graph.RemoveNode(edge.Origin);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge));
    Assert.That(edge, Is.Invalid);
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
  public void RemoveNodesWhere_RemovesExpectedNodes()
  {
    // ARRANGE
    var data = new[] { -1, int.MinValue, 1, 0xC0FFEE };
    var (graph, _) = GraphWithNodesWithData(data);
    bool Predicate(int value) => value <= 0;

    // ACT
    graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(graph.Nodes.Select(node => node.Data), Is.All.Positive);
    Assert.That(graph.Nodes, Has.Count.EqualTo(data.Count(value => !Predicate(value))));
  }

  [Test]
  public void RemoveNodesWhere_ReturnsNumberOfRemovedNodes()
  {
    // ARRANGE
    var data = new[] { -1, int.MinValue, 1, 0xC0FFEE };
    var (graph, nodes) = GraphWithNodesWithData(data);
    bool Predicate(int value) => value <= 0;
    var expectedRemovedNodeCount = data.Count(Predicate);

    // ACT
    var removedNodeCount = graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(removedNodeCount, Is.EqualTo(expectedRemovedNodeCount));
  }

  [Test]
  public void RemoveNodesWhere_AdjacentEdgesAreRemovedAndInvalidated()
  {
    // ARRANGE
    var (graph, edge) = GraphWithEdge;
    edge.Origin.Data = 0;
    edge.Destination.Data = 1;

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge));
    Assert.That(edge, Is.Invalid);
  }

  [Test]
  public void RemoveNodesWhere_RemovedNodesAreInvalidated()
  {
    // ARRANGE
    var data = new[] { -1, int.MinValue, 1, 0xC0FFEE };
    var (graph, nodes) = GraphWithNodesWithData(data);
    bool Predicate(int value) => value <= 0;

    // ACT
    graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(nodes.Where(node => Predicate(node.Data)), AreAll.Invalid);
  }

  [Test]
  public void RemoveEdgesWhere_RemovesExpectedEdges()
  {
    // ARRANGE
    var data = new[] { -1, int.MinValue, 1, 0xC0FFEE };
    var (graph, _) = GraphWithEdgesWithData(data);
    bool Predicate(int value) => value <= 0;

    // ACT
    graph.RemoveEdgesWhere(Predicate);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => edge.Data), Is.All.Positive);
    Assert.That(graph.Edges, Has.Count.EqualTo(data.Count(value => !Predicate(value))));
  }

  [Test]
  public void RemoveEdgesWhere_RemovedEdgesAreInvalidated()
  {
    // ARRANGE
    var data = new[] { -1, int.MinValue, 1, 0xC0FFEE };
    var (graph, edges) = GraphWithEdgesWithData(data);
    bool Predicate(int value) => value <= 0;

    // ACT
    graph.RemoveEdgesWhere(Predicate);

    // ASSERT
    Assert.That(edges.Where(edge => Predicate(edge.Data)), AreAll.Invalid);
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
    var (graph, edge) = GraphWithEdge;

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(edge, Is.Invalid);
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

  [Test]
  public void Clear_NodesAreRemoved()
  {
    // ARRANGE
    var (graph, _) = GraphWithNodes;

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
  }

  [Test]
  public void Clear_NodesAreInvalidated()
  {
    // ARRANGE
    var (graph, nodes) = GraphWithNodes;

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(nodes, AreAll.Invalid);
  }

  [Test]
  public void Clear_EdgesAreRemoved()
  {
    // ARRANGE
    var (graph, _) = GraphWithEdges;

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Clear_EdgesAreInvalidated()
  {
    // ARRANGE
    var (graph, edges) = GraphWithEdges;

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edges, AreAll.Invalid);
  }
}