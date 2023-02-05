using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
// ReSharper disable once InconsistentNaming
public abstract class IUnindexedGraphTests<TGraph> where TGraph : IUnindexedGraph<int, int>
{
  /// <summary> An empty graph. </summary>
  protected abstract TGraph EmptyGraph { get; }

  /// <summary> An empty graph and a node that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, Node<int, int> removedNode) EmptyGraphWithRemovedNode { get; }

  /// <summary> A single node inside a graph not obtainable by other setup methods. </summary>
  protected abstract Node<int, int> NodeFromOtherGraph { get; }

  [Test]
  public void AddNode_AddedNodeHasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = EmptyGraph;

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
    Assert.That(graph.Nodes, Does.Contain(node));
  }

  [Test]
  public void AddNode_GraphContainsNode()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ACT
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node));
  }

  [Test]
  public void AddNodes_AddedNodesHaveExpectedData()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var graph = EmptyGraph;

    // ACT
    var nodes = graph.AddNodes(data).ToArray();

    // ASSERT
    Assert.That(nodes.Select(node => node.Data), Is.EquivalentTo(data));
  }

  [Test]
  public void AddNodes_ValidNodes_GraphContainsNodes()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var graph = EmptyGraph;

    // ACT
    var nodes = graph.AddNodes(data).ToArray();

    // ASSERT
    Assert.That(graph.Nodes, Is.EquivalentTo(nodes));
  }

  [Test]
  public void AddEdge_ValidNodes_AddedEdgeHasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = EmptyGraph;

    // ACT
    var edge = graph.AddEdge(graph.AddNode(default), graph.AddNode(default), data);

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_ValidNodes_GraphContainsEdge()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = EmptyGraph;

    // ACT
    var edge = graph.AddEdge(graph.AddNode(default), graph.AddNode(default), data);

    // ASSERT
    Assert.That(graph.Edges, Does.Contain(edge));
  }

  [Test]
  public void AddEdge_UnexpectedNode_ThrowsArgumentException()
  {
    // ARRANGE
    var (graph, removedNode) = EmptyGraphWithRemovedNode;

    // ASSERT
    Assert.That(() => graph.AddEdge(graph.AddNode(default), NodeFromOtherGraph, default), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(NodeFromOtherGraph, graph.AddNode(default), default), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(graph.AddNode(default), removedNode, default), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(removedNode, graph.AddNode(default), default), Throws.ArgumentException);
  }

  [Test]
  public void TryAddEdge_ValidNodes_ReturnsTrue()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ASSERT
    Assert.That(
      graph.TryAddEdge(graph.AddNode(default), graph.AddNode(default), default, out _),
      Is.True);
  }

  [Test]
  public void TryAddEdge_AddedEdge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = EmptyGraph;

    // ACT
    graph.TryAddEdge(graph.AddNode(default), graph.AddNode(default), data, out var edge);

    // ASSERT
    Assert.That(edge, Is.Not.Null);
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddEdge_GraphContainsEdge()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = EmptyGraph;

    // ACT
    graph.TryAddEdge(graph.AddNode(default), graph.AddNode(default), data, out var edge);

    // ASSERT
    Assert.That(graph.Edges, Does.Contain(edge));
  }

  [Test]
  public void TryAddEdge_UnexpectedNode_ReturnsFalse()
  {
    // ARRANGE
    var (graph, removedNode) = EmptyGraphWithRemovedNode;

    // ASSERT
    Assert.That(graph.TryAddEdge(graph.AddNode(default), NodeFromOtherGraph, default, out _), Is.False);
    Assert.That(graph.TryAddEdge(NodeFromOtherGraph, graph.AddNode(default), default, out _), Is.False);
    Assert.That(graph.TryAddEdge(graph.AddNode(default), removedNode, default, out _), Is.False);
    Assert.That(graph.TryAddEdge(removedNode, graph.AddNode(default), default, out _), Is.False);
  }
}