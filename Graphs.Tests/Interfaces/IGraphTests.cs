using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
// ReSharper disable once InconsistentNaming
public abstract class IGraphTests<TGraph> where TGraph : IGraph<int, int>
{
  /// <summary> A graph and a node expected to be in the graph. </summary>
  protected abstract (TGraph graph, INode<int, int> expectedNode) GraphWithNode { get; }

  /// <summary> A graph and an edge expected to be in the graph. </summary>
  protected abstract (TGraph graph, IEdge<int, int> expectedEdge) GraphWithEdge { get; }

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
    var (graph, expectedNode) = GraphWithNode;

    // ACT
    graph.RemoveNode(expectedNode);

    // ASSERT
    Assert.That(expectedNode.IsValid, Is.False);
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
    Assert.That(expectedEdge.IsValid, Is.False);
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