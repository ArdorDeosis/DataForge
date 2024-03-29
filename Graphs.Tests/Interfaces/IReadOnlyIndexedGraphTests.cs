﻿using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Interfaces;

[TestFixture]
// ReSharper disable once InconsistentNaming
public abstract class IReadOnlyIndexedGraphTests<TGraph> where TGraph : IReadOnlyIndexedGraph<int, int, int>
{
  [OneTimeSetUp]
  public void EnsureUnrelatedIndex()
  {
    Assert.That(GraphWithNode.expectedNode.Index, NUnit.Framework.Is.Not.EqualTo(UnrelatedIndex));
    Assert.That(GraphWithNodeIndices.expectedIndices, Does.Not.Contain(UnrelatedIndex));
    Assert.That(EmptyGraphWithRemovedNodeIndex.removedIndex, NUnit.Framework.Is.Not.EqualTo(UnrelatedIndex));
  }

  private const int UnrelatedIndex = 1375610840;

  /// <summary> An empty graph. </summary>
  protected abstract TGraph EmptyGraph { get; }

  /// <summary> A graph and a node in the graph. </summary>
  protected abstract (TGraph graph, IndexedNode<int, int, int> expectedNode) GraphWithNode { get; }

  /// <summary> A graph and the node indices expected to be in the graph. </summary>
  protected abstract (TGraph graph, int[] expectedIndices) GraphWithNodeIndices { get; }

  /// <summary> An empty graph and a node index that has been in the graph but was removed. </summary>
  protected abstract (TGraph graph, int removedIndex) EmptyGraphWithRemovedNodeIndex { get; }

  [Test]
  public void Indices_EmptyGraph_IsEmpty()
  {
    // ARRANGE
    var graph = EmptyGraph;

    // ASSERT
    Assert.That(graph.Indices, NUnit.Framework.Is.Empty);
  }

  [Test]
  public void Indices_ContainsExpectedIndices()
  {
    // ARRANGE
    var (graph, expectedIndices) = GraphWithNodeIndices;

    // ASSERT
    Assert.That(graph.Indices, NUnit.Framework.Is.EquivalentTo(expectedIndices));
  }

  [Test]
  public void Indices_DoesNotContainUnexpectedIndices()
  {
    // ARRANGE
    var (graph, removedIndex) = EmptyGraphWithRemovedNodeIndex;

    // ASSERT
    Assert.That(graph.Nodes, Does.Not.Contain(removedIndex));
    Assert.That(graph.Nodes, Does.Not.Contain(UnrelatedIndex));
  }

  [Test]
  public void GetNode_ExistingIndex_ReturnsNode()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ASSERT
    Assert.That(graph[expectedNode.Index], NUnit.Framework.Is.EqualTo(expectedNode));
    Assert.That(graph.GetNode(expectedNode.Index), NUnit.Framework.Is.EqualTo(expectedNode));
  }

  [Test]
  public void GetNode_UnexpectedIndices_ThrowsKeyNotFoundException()
  {
    // ARRANGE
    var (graph, removedIndex) = EmptyGraphWithRemovedNodeIndex;

    // ASSERT
    Assert.That(() => graph[removedIndex], Throws.Exception.TypeOf<KeyNotFoundException>());
    Assert.That(() => graph[UnrelatedIndex], Throws.Exception.TypeOf<KeyNotFoundException>());
    Assert.That(() => graph.GetNode(removedIndex), Throws.Exception.TypeOf<KeyNotFoundException>());
    Assert.That(() => graph.GetNode(UnrelatedIndex), Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNodeOrNull_ExistingIndex_ReturnsNode()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ASSERT
    Assert.That(graph.GetNodeOrNull(expectedNode.Index), NUnit.Framework.Is.EqualTo(expectedNode));
  }

  [Test]
  public void GetNodeOrNull_UnexpectedIndices_ReturnsNull()
  {
    // ARRANGE
    var (graph, removedIndex) = EmptyGraphWithRemovedNodeIndex;

    // ASSERT
    Assert.That(graph.GetNodeOrNull(removedIndex), NUnit.Framework.Is.Null);
    Assert.That(graph.GetNodeOrNull(UnrelatedIndex), NUnit.Framework.Is.Null);
  }

  [Test]
  public void TryGetNode_ExistingIndex_ReturnsTrue()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ASSERT
    Assert.That(graph.TryGetNode(expectedNode.Index, out _), NUnit.Framework.Is.True);
  }

  [Test]
  public void TryGetNode_ExistingIndex_OutputsExpectedNode()
  {
    // ARRANGE
    var (graph, expectedNode) = GraphWithNode;

    // ACT
    graph.TryGetNode(expectedNode.Index, out var node);

    // ASSERT
    Assert.That(node, NUnit.Framework.Is.EqualTo(expectedNode));
  }

  [Test]
  public void TryGetNode_UnexpectedIndices_ReturnsFalse()
  {
    // ARRANGE
    var (graph, removedIndex) = EmptyGraphWithRemovedNodeIndex;

    // ASSERT
    Assert.That(graph.TryGetNode(removedIndex, out _), NUnit.Framework.Is.False);
    Assert.That(graph.TryGetNode(UnrelatedIndex, out _), NUnit.Framework.Is.False);
  }

  [Test]
  public void ContainsIndex_ContainedIndex_True()
  {
    // ARRANGE
    var (graph, expectedIndices) = GraphWithNodeIndices;

    // ASSERT
    foreach (var index in expectedIndices)
      Assert.That(graph.Contains(index), NUnit.Framework.Is.True);
  }

  [Test]
  public void ContainsIndex_NotContainedIndex_False()
  {
    // ARRANGE
    var (graph, removedIndex) = EmptyGraphWithRemovedNodeIndex;

    // ASSERT
    Assert.That(graph.Contains(removedIndex), NUnit.Framework.Is.False);
    Assert.That(graph.Contains(UnrelatedIndex), NUnit.Framework.Is.False);
  }
}