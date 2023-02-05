using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
// ReSharper disable once InconsistentNaming
public abstract class IAutoIndexedGraphTests<TGraph> where TGraph : IAutoIndexedGraph<int, int, int>
{
  /// <summary> An empty graph using an index provider that uses the node's data as index. </summary>
  protected abstract TGraph EmptyGraphTakingIndicesFromData { get; }

  /// <summary> A graph using an index provider that uses the node's data as index and a node in the graph. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> expectedNode) GraphWithNodeTakingIndicesFromData { get; }

  [Test]
  public void AddNode_ValidIndex_ReturnsNodeWithCorrectData()
  {
    // ARRANGE
    var graph = EmptyGraphTakingIndicesFromData;
    const int data = 0xBEEF;

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node, Is.Valid);
    Assert.That(node.Index, Is.EqualTo(data));
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_ValidIndex_GraphContainsNode()
  {
    // ARRANGE
    var graph = EmptyGraphTakingIndicesFromData;

    // ACT
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node));
    Assert.That(graph.Contains(node), Is.True);
    Assert.That(graph.Contains(node.Index), Is.True);
  }

  [Test]
  public void AddNode_ExistingIndex_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var (graph, node) = GraphWithNodeTakingIndicesFromData;

    // ASSERT
    Assert.That(() => graph.AddNode(node.Index), Throws.Exception.TypeOf<InvalidOperationException>());
  }

  [Test]
  public void TryAddNode_ValidIndex_ReturnsTrue()
  {
    // ARRANGE
    var graph = EmptyGraphTakingIndicesFromData;

    // ASSERT
    Assert.That(graph.TryAddNode(default, out _), Is.True);
  }

  [Test]
  public void TryAddNode_ValidIndex_OutputsNodeWithCorrectData()
  {
    // ARRANGE
    var graph = EmptyGraphTakingIndicesFromData;
    const int data = 0xBEEF;

    // ACT
    graph.TryAddNode(data, out var node);

    // ASSERT
    Assert.That(node, Is.Valid);
    Assert.That(node.Index, Is.EqualTo(data));
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddNode_ValidIndex_GraphContainsNode()
  {
    // ARRANGE
    var graph = EmptyGraphTakingIndicesFromData;

    // ACT
    graph.TryAddNode(default, out var node);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node));
    Assert.That(graph.Contains(node), Is.True);
    Assert.That(graph.Contains(node.Index), Is.True);
  }

  [Test]
  public void TryAddNode_ExistingIndex_ReturnsFalse()
  {
    // ARRANGE
    var (graph, node) = GraphWithNodeTakingIndicesFromData;

    // ASSERT
    Assert.That(graph.TryAddNode(node.Index, out _), Is.False);
  }
}