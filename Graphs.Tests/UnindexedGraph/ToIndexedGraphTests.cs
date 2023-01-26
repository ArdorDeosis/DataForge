using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

[TestFixture]
public class ToIndexedGraphTests
{
  [Test]
  public void ToIndexedGraph_IndexEqualityComparer_EqualityComparerIsUsed()
  {
    // ARRANGE
    var indices = TestEqualityComparer.EquivalentIndexArray;
    var graph = new Graph<int, int>();
    graph.AddNodes(indices);
    var equalityComparer = new TestEqualityComparer();

    // ASSERT
    Assert.That(() => graph.ToIndexedGraph(data => data, equalityComparer), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), equalityComparer), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, () => new TestEqualityComparer()), Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), () => new TestEqualityComparer()),
      Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, data => data, data => data, equalityComparer),
      Throws.Exception);
    Assert.That(
      () => graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data, equalityComparer),
      Throws.Exception);
    Assert.That(() => graph.ToIndexedGraph(data => data, data => data, data => data, () => new TestEqualityComparer()),
      Throws.Exception);
    Assert.That(
      () => graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data,
        () => new TestEqualityComparer()), Throws.Exception);
  }

  [Test]
  public void ToIndexedGraph_GeneratorFunction_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
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
      Assert.That(indexedGraph.Indices, Is.EquivalentTo(indices));
      foreach (var index in indices)
        Assert.That(indexedGraph[index].Data, Is.EqualTo(index));
    }
  }

  [Test]
  public void ToIndexedGraph_IndexGenerator_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    graph.AddNodes(data);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>()),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Indices, Is.EquivalentTo(new[] { 0, 1 }));
      Assert.That(indexedGraph[0].Data, Is.EqualTo(data[0]));
      Assert.That(indexedGraph[1].Data, Is.EqualTo(data[1]));
    }
  }

  [Test]
  public void ToIndexedGraph_NoNodeDataCloneMethod_NodeDataIsClonedCorrectly()
  {
    // ARRANGE
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
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
      Assert.That(indexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(nodeData));
      foreach (var originalNode in graph.Nodes)
      {
        var clonedNode = indexedGraph[originalNode.Data];
        var expectedData = originalNode.Data;
        Assert.That(clonedNode.Data, Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_NodeDataCloneMethod_NodeDataIsClonedCorrectly()
  {
    // ARRANGE
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
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
      Assert.That(indexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(nodeData.Select(CloneData)));
      foreach (var originalNode in graph.Nodes)
      {
        var clonedNode = indexedGraph[originalNode.Data];
        var expectedData = CloneData(originalNode.Data);
        Assert.That(clonedNode.Data, Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_NoEdgeDataCloneMethod_EdgeDataIsClonedCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    var nodes = graph.AddNodes(indices).ToArray();
    graph.AddEdge(nodes[0], nodes[1], edgeData[0]);
    graph.AddEdge(nodes[1], nodes[0], edgeData[1]);

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data),
      graph.ToIndexedGraph(data => data, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>()),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(edgeData));
      foreach (var originalEdge in graph.Edges)
      {
        var clonedEdge = indexedGraph.Edges.First(edge => edge.OriginIndex == originalEdge.Origin.Data);
        var expectedData = originalEdge.Data;
        Assert.That(clonedEdge.Data, Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void ToIndexedGraph_EdgeDataCloneMethod_EdgeDataIsClonedCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    var nodes = graph.AddNodes(indices).ToArray();
    graph.AddEdge(nodes[0], nodes[1], edgeData[0]);
    graph.AddEdge(nodes[1], nodes[0], edgeData[1]);

    int CloneData(int data) => -data;

    // ACT
    var indexedGraphs = new[]
    {
      graph.ToIndexedGraph(data => data, CloneData, CloneData),
      graph.ToIndexedGraph(data => data, CloneData, CloneData, () => EqualityComparer<int>.Default),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), CloneData, CloneData),
      graph.ToIndexedGraph(new IncrementalIndexProvider<int>(), CloneData, CloneData,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(edgeData.Select(CloneData)));
      foreach (var originalEdge in graph.Edges)
      {
        var clonedEdge = indexedGraph.Edges.First(edge => edge.OriginIndex == originalEdge.Origin.Data);
        var expectedData = CloneData(originalEdge.Data);
        Assert.That(clonedEdge.Data, Is.EqualTo(expectedData));
      }
    }
  }
}