using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

[TestFixture]
public class TransformToIndexedGraphTests
{
  [Test]
  public void TransformToIndexedGraph_IndexEqualityComparer_EqualityComparerIsUsed()
  {
    // ARRANGE
    var indices = TestEqualityComparer.EquivalentIndexArray;
    var graph = new Graph<int, int>();
    graph.AddNodes(indices);
    var equalityComparer = new TestEqualityComparer();

    // ASSERT
    Assert.That(
      () => graph.TransformToIndexedGraph(data => data, data => data, data => data, equalityComparer),
      Throws.Exception);
    Assert.That(
      () => graph.TransformToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data,
        equalityComparer), Throws.Exception);
    Assert.That(
      () => graph.TransformToIndexedGraph(data => data, data => data, data => data, () => new TestEqualityComparer()),
      Throws.Exception);
    Assert.That(
      () => graph.TransformToIndexedGraph(new IncrementalIndexProvider<int>(), data => data, data => data,
        () => new TestEqualityComparer()), Throws.Exception);
  }

  [Test]
  public void TransformToIndexedGraph_GeneratorFunction_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    graph.AddNodes(indices);

    string TransformIndex(int index) => index.ToString();

    // ACT
    var indexedGraphs = new[]
    {
      graph.TransformToIndexedGraph(TransformIndex, data => data, data => data),
      graph.TransformToIndexedGraph(TransformIndex, data => data, data => data, () => EqualityComparer<string>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Indices, Is.EquivalentTo(indices.Select(TransformIndex)));
      foreach (var index in indices)
        Assert.That(indexedGraph[TransformIndex(index)].Data, Is.EqualTo(index));
    }
  }

  [Test]
  public void TransformToIndexedGraph_IndexGenerator_IndicesAreSetCorrectly()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    graph.AddNodes(data);

    string TransformIndex(int index) => index.ToString();

    // ACT
    var indexedGraphs = new[]
    {
      graph.TransformToIndexedGraph(new StatelessIndexProvider<int, string>(TransformIndex), data => data,
        data => data),
      graph.TransformToIndexedGraph(new StatelessIndexProvider<int, string>(TransformIndex), data => data, data => data,
        () => EqualityComparer<string>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Indices, Is.EquivalentTo(data.Select(value => value.ToString())));
      foreach (var value in data)
        Assert.That(indexedGraph[TransformIndex(value)].Data, Is.EqualTo(value));
    }
  }

  [Test]
  public void TransformToIndexedGraph_NodeDataCloneMethod_NodeDataIsClonedCorrectly()
  {
    // ARRANGE
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    graph.AddNodes(nodeData);


    string TransformData(int index) => index.ToString();

    // ACT
    var indexedGraphs = new[]
    {
      graph.TransformToIndexedGraph(data => data, TransformData, TransformData),
      graph.TransformToIndexedGraph(data => data, TransformData, TransformData, () => EqualityComparer<int>.Default),
      graph.TransformToIndexedGraph(new StatelessIndexProvider<int, int>(data => data), TransformData, TransformData),
      graph.TransformToIndexedGraph(new StatelessIndexProvider<int, int>(data => data), TransformData, TransformData,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(nodeData.Select(TransformData)));
      foreach (var originalNode in graph.Nodes)
      {
        var clonedNode = indexedGraph[originalNode.Data];
        var expectedData = TransformData(originalNode.Data);
        Assert.That(clonedNode.Data, Is.EqualTo(expectedData));
      }
    }
  }

  [Test]
  public void TransformToIndexedGraph_EdgeDataCloneMethod_EdgeDataIsClonedCorrectly()
  {
    // ARRANGE
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();
    var nodes = graph.AddNodes(indices).ToArray();
    graph.AddEdge(nodes[0], nodes[1], edgeData[0]);
    graph.AddEdge(nodes[1], nodes[0], edgeData[1]);

    string TransformData(int data) => data.ToString();

    // ACT
    var indexedGraphs = new[]
    {
      graph.TransformToIndexedGraph(data => data, TransformData, TransformData),
      graph.TransformToIndexedGraph(data => data, TransformData, TransformData, () => EqualityComparer<int>.Default),
      graph.TransformToIndexedGraph(new IncrementalIndexProvider<int>(), TransformData, TransformData),
      graph.TransformToIndexedGraph(new IncrementalIndexProvider<int>(), TransformData, TransformData,
        () => EqualityComparer<int>.Default),
    };

    // ASSERT
    foreach (var indexedGraph in indexedGraphs)
    {
      Assert.That(indexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(edgeData.Select(TransformData)));
      foreach (var originalEdge in graph.Edges)
      {
        var clonedEdge = indexedGraph.Edges.First(edge => edge.OriginIndex == originalEdge.Origin.Data);
        var expectedData = TransformData(originalEdge.Data);
        Assert.That(clonedEdge.Data, Is.EqualTo(expectedData));
      }
    }
  }
}