using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
// ReSharper disable once InconsistentNaming
public abstract class IManuallyIndexedGraphTests<TGraph> where TGraph : IManuallyIndexedGraph<int, int, int>
{
  /// <summary> An empty graph. </summary>
  protected abstract TGraph EmptyGraph { get; }

  /// <summary> A graph and a node expected to be in the graph. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> expectedNode) GraphWithNode { get; }

  [Test]
  public void AddNode_ValidIndex_ReturnsNodeWithCorrectData()
  {
    // ARRANGE
    var graph = EmptyGraph;
    const int index = 0xC0FFEE;
    const int data = 0xBEEF;

    // ACT
    var node = graph.AddNode(index, data);

    // ASSERT
    Assert.That(node, Is.Valid);
    Assert.That(node.Index, Is.EqualTo(index));
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_ValidIndex_GraphContainsNode()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ACT
    var node = graph.AddNode(default, default);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node));
    Assert.That(graph.Contains(node), Is.True);
    Assert.That(graph.Contains(node.Index), Is.True);
  }

  [Test]
  public void AddNode_ExistingIndex_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ASSERT
    Assert.That(() => graph.AddNode(node.Index, default), Throws.Exception.TypeOf<InvalidOperationException>());
  }

  [Test]
  public void TryAddNode_ValidIndex_ReturnsTrue()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ASSERT
    Assert.That(graph.TryAddNode(default, default, out _), Is.True);
  }

  [Test]
  public void TryAddNode_ValidIndex_OutputsNodeWithCorrectData()
  {
    // ARRANGE
    var graph = EmptyGraph;
    const int index = 0xC0FFEE;
    const int data = 0xBEEF;

    // ACT
    graph.TryAddNode(index, data, out var node);

    // ASSERT
    Assert.That(node, Is.Valid);
    Assert.That(node.Index, Is.EqualTo(index));
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddNode_ValidIndex_GraphContainsNode()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ACT
    graph.TryAddNode(default, default, out var node);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node));
    Assert.That(graph.Contains(node), Is.True);
    Assert.That(graph.Contains(node.Index), Is.True);
  }

  [Test]
  public void TryAddNode_ExistingIndex_ReturnsFalse()
  {
    // ARRANGE
    var (graph, node) = GraphWithNode;

    // ASSERT
    Assert.That(graph.TryAddNode(node.Index, default, out _), Is.False);
  }
}