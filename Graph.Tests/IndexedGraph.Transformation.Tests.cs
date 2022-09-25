using System.Linq;
using NUnit.Framework;

namespace Graph.Tests;

public class IndexedGraphTransformationTests
{
  private readonly TestData noData = new() { Value = 0, Marker = 0 };
  private readonly TestData data1 = new() { Value = 1, Marker = 1 };
  private readonly TestData data2 = new() { Value = 2, Marker = 2 };
  private readonly TestData data3 = new() { Value = 3, Marker = 3 };

  private static TestData MakeCopy(TestData data) => new() { Value = data.Value, Marker = data.Marker };

  private static TestData NegateValue(TestData data) => new() { Value = -data.Value, Marker = data.Marker };

  [Test]
  public void Copy_CopiesNodes()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddNode(1, data1);
    graph.AddNode(2, data2);

    // ACT
    var copy = graph.Copy();

    // ASSERT
    Assert.That(copy.Nodes.Data(), Is.EquivalentTo(new[] { data1, data2 }));
  }

  [Test]
  public void Copy_CopiesIndices()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddNode(1, noData);
    graph.AddNode(2, noData);

    // ACT
    var copy = graph.Copy();

    // ASSERT
    Assert.That(copy.Indices, Is.EquivalentTo(new[] { 1, 2 }));
  }

  [Test]
  public void Copy_CopiesEdges()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    var startNode = graph.AddNode(1, data1);
    var endNode = graph.AddNode(2, data2);
    graph.AddEdge(startNode, endNode, data3);

    // ACT
    var copy = graph.Copy();

    // ASSERT
    Assert.That(copy.Edges.Data(), Is.EquivalentTo(new[] { data3 }));
    Assert.That(copy.Edges.First().Start.Data, Is.EqualTo(data1));
    Assert.That(copy.Edges.First().End.Data, Is.EqualTo(data2));
  }

  [Test]
  public void Copy_WithCopyLogic_NodeCopyLogicIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddNode(1, data1);

    // ACT
    var copy = graph.Copy(MakeCopy, MakeCopy);

    // ASSERT
    Assert.That(copy[1].Data, Is.Not.EqualTo(data1));
    Assert.That(copy[1].Data.Value, Is.EqualTo(data1.Value));
  }

  [Test]
  public void Copy_WithCopyLogic_EdgeCopyLogicIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();

    graph.AddEdge(graph.AddNode(1, noData), graph.AddNode(2, noData), data1);

    // ACT
    var copy = graph.Copy(MakeCopy, MakeCopy);

    // ASSERT
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data, Is.Not.EqualTo(data1));
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(data1.Value));
  }

  [Test]
  public void Transform_NodeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    var node1 = graph.AddNode(1, data1);

    // ACT
    var copy = graph.Transform(NegateValue, MakeCopy);

    // ASSERT
    Assert.That(copy[1].Data.Value, Is.EqualTo(NegateValue(node1.Data).Value));
  }

  [Test]
  public void Transform_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddEdge(graph.AddNode(1, noData), graph.AddNode(2, noData), data1);

    // ACT
    var copy = graph.Transform(MakeCopy, NegateValue);

    // ASSERT
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
  }

  [Test]
  public void TransformIncludingIndex_NodeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    var node1 = graph.AddNode(1, data1);

    // ACT
    var copy = graph.Transform(NegateValue, MakeCopy, n => n);

    // ASSERT
    Assert.That(copy[1].Data.Value, Is.EqualTo(NegateValue(node1.Data).Value));
  }

  [Test]
  public void TransformIncludingIndex_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddEdge(graph.AddNode(1, noData), graph.AddNode(2, noData), data1);

    // ACT
    var copy = graph.Transform(MakeCopy, NegateValue, n => n);

    // ASSERT
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
  }

  [Test]
  public void TransformIncludingIndex_IndexIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddNode(1, data1);

    // ACT
    var copy = graph.Transform(NegateValue, MakeCopy, n => n * 2);

    // ASSERT
    Assert.That(copy.Indices, Is.EquivalentTo(new[] { 2 }));
  }

  [Test]
  public void TransformIncludingIndex_IndexCollision_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();
    graph.AddNode(1, data1);
    graph.AddNode(2, data1);

    // ACT + ASSERT
    Assert.That(() => graph.Transform(NegateValue, MakeCopy, n => 0xC0FFEE), Throws.InvalidOperationException);
  }

  [Test]
  public void ToNonIndexedGraph_ContainsAllNodes()
  {
    // ARRANGE
    var indexedGraph = new IndexedGraph<int, TestData, TestData>();
    indexedGraph.AddNode(1, data1);
    indexedGraph.AddNode(2, data2);

    // ACT 
    var graph = indexedGraph.ToNonIndexedGraph();

    // ASSERT
    Assert.That(graph.Nodes.Data(), Is.EquivalentTo(indexedGraph.Nodes.Data()));
  }

  [Test]
  public void ToNonIndexedGraph_ContainsAllEdges()
  {
    // ARRANGE
    var indexedGraph = new IndexedGraph<int, TestData, TestData>();
    indexedGraph.AddEdge(indexedGraph.AddNode(1, data1), indexedGraph.AddNode(2, data2), data3);

    // ACT 
    var graph = indexedGraph.ToNonIndexedGraph();

    // ASSERT
    Assert.That(graph.Edges.Data(), Is.EquivalentTo(indexedGraph.Edges.Data()));
    Assert.That(graph.Edges.First().Start.Data, Is.EqualTo(indexedGraph.Edges.First().Start.Data));
    Assert.That(graph.Edges.First().End.Data, Is.EqualTo(indexedGraph.Edges.First().End.Data));
  }

  [Test]
  public void ToNonIndexedGraph_WithCopyLogic_NodeCopyLogicIsUsed()
  {
    // ARRANGE
    var indexedGraph = new IndexedGraph<int, TestData, TestData>();
    indexedGraph.AddNode(1, data1);

    // ACT
    var graph = indexedGraph.ToNonIndexedGraph(MakeCopy, MakeCopy);

    // ASSERT
    Assert.That(graph.Nodes.WithMarker(data1.Marker).Data, Is.Not.EqualTo(data1));
    Assert.That(graph.Nodes.WithMarker(data1.Marker).Data.Value, Is.EqualTo(data1.Value));
  }

  [Test]
  public void ToNonIndexedGraph_WithCopyLogic_EdgeCopyLogicIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, TestData, TestData>();

    graph.AddEdge(graph.AddNode(1, noData), graph.AddNode(2, noData), data1);

    // ACT
    var copy = graph.Copy(MakeCopy, MakeCopy);

    // ASSERT
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data, Is.Not.EqualTo(data1));
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(data1.Value));
  }

  [Test]
  public void TransformToNonIndexedGraph_NodeCopyLogicIsUsed()
  {
    // ARRANGE
    var indexedGraph = new IndexedGraph<int, TestData, TestData>();
    indexedGraph.AddNode(1, data1);

    // ACT
    var graph = indexedGraph.ToNonIndexedGraph(NegateValue, MakeCopy);

    // ASSERT
    Assert.That(graph.Nodes.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
  }

  [Test]
  public void TransformToNonIndexedGraph_EdgeCopyLogicIsUsed()
  {
    // ARRANGE
    var indexedGraph = new IndexedGraph<int, TestData, TestData>();

    indexedGraph.AddEdge(indexedGraph.AddNode(1, noData), indexedGraph.AddNode(2, noData), data1);

    // ACT
    var graph = indexedGraph.TransformToNonIndexedGraph(MakeCopy, NegateValue);

    // ASSERT
    Assert.That(graph.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
  }
}