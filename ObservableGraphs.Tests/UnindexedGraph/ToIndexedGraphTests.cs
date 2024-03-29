﻿using DataForge.Graphs.Tests;
using NUnit.Framework;

namespace DataForge.Graphs.Observable.Tests.UnindexedGraph;

internal class ToIndexedGraphTests
{
  [Test]
  public void ToIndexedGraph_IndexEqualityComparer_EqualityComparerIsUsed()
  {
    // ARRANGE
    var (index1, index2) = TestEqualityComparer.EquivalentIndexPair;
    var graph = new ObservableGraph<int, int>();
    graph.AddNodes(index1, index2);
    var equalityComparer = new TestEqualityComparer();

    // ASSERT
    Assert.That(() => graph.ToIndexedGraph(data => data, equalityComparer), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), equalityComparer),
      Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, () => new TestEqualityComparer()), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), () => new TestEqualityComparer()),
      Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, data => data, data => data, equalityComparer),
      Throws.Exception);
    Assert.That(
      () => graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), data => data, data => data,
        equalityComparer), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, data => data, data => data, () => new TestEqualityComparer()),
      Throws.Exception);
    Assert.That(
      () => graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), data => data, data => data,
        () => new TestEqualityComparer()), Throws.Exception);
  }

  [Test]
  public void ToIndexedGraph_GeneratorFunction_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    graph.AddNodes(indices);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data),
      graph.ToIndexedGraph(data => data, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(data => data, data => data, data => data),
      graph.ToIndexedGraph(data => data, data => data, data => data, () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Indices, NUnit.Framework.Is.EquivalentTo(indices));
      foreach (var index in indices)
        Assert.That(indexedGraph[index].Data, NUnit.Framework.Is.EqualTo(index));
    }
  }

  [Test]
  public void ToIndexedGraph_IndexGenerator_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    graph.AddNodes(data);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0)),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), data => data, data => data),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), data => data, data => data,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Indices, NUnit.Framework.Is.EquivalentTo(new[] { 0, 1 }));
      Assert.That(indexedGraph[0].Data, NUnit.Framework.Is.EqualTo(data[0]));
      Assert.That(indexedGraph[1].Data, NUnit.Framework.Is.EqualTo(data[1]));
    }
  }

  [Test]
  public void ToIndexedGraph_NoNodeDataCloneMethod_NodeDataIsClonedCorrectly()
  {
    // ARRANGE
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    graph.AddNodes(nodeData);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data),
      graph.ToIndexedGraph(data => data, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new StatelessIndexProvider<int, int>(data => data)),
      graph.ToIndexedGraph(new StatelessIndexProvider<int, int>(data => data), () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Nodes.Select(node => node.Data), NUnit.Framework.Is.EquivalentTo(nodeData));
      foreach (var originalNode in graph.Nodes)
      {
        var clonedNode = indexedGraph[originalNode.Data];
        var expectedData = originalNode.Data;
        Assert.That(clonedNode.Data, NUnit.Framework.Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_NodeDataCloneMethod_NodeDataIsClonedCorrectly()
  {
    // ARRANGE
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    graph.AddNodes(nodeData);

    int CloneData(int data) => -data;

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data, CloneData, CloneData),
      graph.ToIndexedGraph(data => data, CloneData, CloneData, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new StatelessIndexProvider<int, int>(data => data), CloneData, CloneData),
      graph.ToIndexedGraph(new StatelessIndexProvider<int, int>(data => data), CloneData, CloneData,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Nodes.Select(node => node.Data),
        NUnit.Framework.Is.EquivalentTo(nodeData.Select(CloneData)));
      foreach (var originalNode in graph.Nodes)
      {
        var clonedNode = indexedGraph[originalNode.Data];
        var expectedData = CloneData(originalNode.Data);
        Assert.That(clonedNode.Data, NUnit.Framework.Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_NoEdgeDataCloneMethod_EdgeDataIsClonedCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    var nodes = graph.AddNodes(indices).ToArray();
    graph.AddEdge(nodes[0], nodes[1], edgeData[0]);
    graph.AddEdge(nodes[1], nodes[0], edgeData[1]);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data),
      graph.ToIndexedGraph(data => data, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0)),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Edges.Select(edge => edge.Data), NUnit.Framework.Is.EquivalentTo(edgeData));
      foreach (var originalEdge in graph.Edges)
      {
        var clonedEdge = indexedGraph.Edges.First(edge => edge.OriginIndex == originalEdge.Origin.Data);
        var expectedData = originalEdge.Data;
        Assert.That(clonedEdge.Data, NUnit.Framework.Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_EdgeDataCloneMethod_EdgeDataIsClonedCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new ObservableGraph<int, int>();
    var nodes = graph.AddNodes(indices).ToArray();
    graph.AddEdge(nodes[0], nodes[1], edgeData[0]);
    graph.AddEdge(nodes[1], nodes[0], edgeData[1]);

    int CloneData(int data) => -data;

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data, CloneData, CloneData),
      graph.ToIndexedGraph(data => data, CloneData, CloneData, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), CloneData, CloneData),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int, int>(0), CloneData, CloneData,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Edges.Select(edge => edge.Data),
        NUnit.Framework.Is.EquivalentTo(edgeData.Select(CloneData)));
      foreach (var originalEdge in graph.Edges)
      {
        var clonedEdge = indexedGraph.Edges.First(edge => edge.OriginIndex == originalEdge.Origin.Data);
        var expectedData = CloneData(originalEdge.Data);
        Assert.That(clonedEdge.Data, NUnit.Framework.Is.EqualTo(expectedData));
      }
    }
  }
}