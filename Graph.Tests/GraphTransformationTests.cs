using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Graph.Tests;

public class GraphTransformationTests
{
  private readonly TestData noData = new() { Value = 0, Marker = 0 };
  private readonly TestData data1 = new() { Value = 1, Marker = 1 };
  private readonly TestData data2 = new() { Value = 2, Marker = 2 };
  private readonly TestData data3 = new() { Value = 3, Marker = 3 };
  private readonly TestData data4 = new() { Value = 4, Marker = 4 };
  private readonly TestData data5 = new() { Value = 5, Marker = 5 };
  private readonly TestData data6 = new() { Value = 6, Marker = 6 };

  private static TestData MakeCopy(TestData data) => new() { Value = data.Value, Marker = data.Marker };

  private static TestData NegateValue(TestData data) => new() { Value = -data.Value, Marker = data.Marker };

  [Test]
  public void Copy_CopiesNodes()
  {
    // ARRANGE
    var graph = new OldGraph<TestData, TestData>();
    graph.AddNode(data1);
    graph.AddNode(data2);

    // ACT
    var copy = graph.Copy();

    // ASSERT
    Assert.That(copy.Nodes.Data(), Is.EquivalentTo(new[] { data1, data2 }));
  }

  [Test]
  public void Copy_CopiesEdges()
  {
    // ARRANGE
    var graph = new OldGraph<TestData, TestData>();
    var startNode = graph.AddNode(data1);
    var endNode = graph.AddNode(data2);
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
    var graph = new OldGraph<TestData, TestData>();
    graph.AddNode(data1);

    // ACT
    var copy = graph.Copy(MakeCopy, MakeCopy);

    // ASSERT
    Assert.That(copy.Nodes.WithMarker(data1.Marker).Data, Is.Not.EqualTo(data1));
    Assert.That(copy.Nodes.WithMarker(data1.Marker).Data.Value, Is.EqualTo(data1.Value));
  }

  [Test]
  public void Copy_WithCopyLogic_EdgeCopyLogicIsUsed()
  {
    // ARRANGE
    var graph = new OldGraph<TestData, TestData>();

    graph.AddEdge(graph.AddNode(noData), graph.AddNode(noData), data1);

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
    var graph = new OldGraph<TestData, TestData>();
    var node1 = graph.AddNode(data1);

    // ACT
    var copy = graph.Transform(NegateValue, MakeCopy);

    // ASSERT
    Assert.That(copy.Nodes.WithMarker(node1.Data.Marker).Data.Value, Is.EqualTo(NegateValue(node1.Data).Value));
  }

  [Test]
  public void Transform_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new OldGraph<TestData, TestData>();
    graph.AddEdge(graph.AddNode(noData), graph.AddNode(noData), data1);

    // ACT
    var copy = graph.Transform(MakeCopy, NegateValue);

    // ASSERT
    Assert.That(copy.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
  }

  [Test]
  public void Merge_ResultContainsAllNodes()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddNodes(data1);
    graph2.AddNodes(data2);

    // ACT
    var merged = graph1.Merge(graph2);

    // ASSERT
    Assert.That(merged.Nodes.Data(), Is.EquivalentTo(new[] { data1, data2 }));
  }

  [Test]
  public void Merge_ResultContainsAllEdges()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddEdge(graph1.AddNode(data3), graph1.AddNode(data4), data1);
    graph2.AddEdge(graph2.AddNode(data5), graph2.AddNode(data6), data2);

    // ACT
    var merged = graph1.Merge(graph2);

    // ASSERT
    Assert.That(merged.Edges.Data(), Is.EquivalentTo(new[] { data1, data2 }));
    Assert.That(merged.Edges.WithMarker(data1.Marker).Start.Data, Is.EqualTo(data3));
    Assert.That(merged.Edges.WithMarker(data1.Marker).End.Data, Is.EqualTo(data4));
    Assert.That(merged.Edges.WithMarker(data2.Marker).Start.Data, Is.EqualTo(data5));
    Assert.That(merged.Edges.WithMarker(data2.Marker).End.Data, Is.EqualTo(data6));
  }

  [Test]
  public void MergeTransform_ResultContainsAllNodes()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddNodes(data1);
    graph2.AddNodes(data2);

    // ACT
    var merged = graph1.MergeTransform(NegateValue, MakeCopy, graph2);

    // ASSERT
    Assert.That(merged.Nodes.Markers(), Is.EquivalentTo(new[] { data1.Marker, data2.Marker }));
  }

  [Test]
  public void MergeTransform_NodeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddNodes(data1);
    graph2.AddNodes(data2);

    // ACT
    var merged = graph1.MergeTransform(NegateValue, MakeCopy, graph2);

    // ASSERT
    Assert.That(merged.Nodes.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
    Assert.That(merged.Nodes.WithMarker(data2.Marker).Data.Value, Is.EqualTo(NegateValue(data2).Value));
  }

  [Test]
  public void MergeTransform_ResultContainsAllEdges()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddEdge(graph1.AddNode(data3), graph1.AddNode(data4), data1);
    graph2.AddEdge(graph2.AddNode(data5), graph2.AddNode(data6), data2);

    // ACT
    var merged = graph1.MergeTransform(MakeCopy, NegateValue, graph2);

    // ASSERT
    Assert.That(merged.Edges.Markers(), Is.EquivalentTo(new[] { data1.Marker, data2.Marker }));
    Assert.That(merged.Edges.WithMarker(data1.Marker).Start.Data.Marker, Is.EqualTo(data3.Marker));
    Assert.That(merged.Edges.WithMarker(data1.Marker).End.Data.Marker, Is.EqualTo(data4.Marker));
    Assert.That(merged.Edges.WithMarker(data2.Marker).Start.Data.Marker, Is.EqualTo(data5.Marker));
    Assert.That(merged.Edges.WithMarker(data2.Marker).End.Data.Marker, Is.EqualTo(data6.Marker));
  }

  [Test]
  public void MergeTransform_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph1 = new OldGraph<TestData, TestData>();
    var graph2 = new OldGraph<TestData, TestData>();

    graph1.AddEdge(graph1.AddNode(noData), graph1.AddNode(noData), data1);
    graph2.AddEdge(graph2.AddNode(noData), graph2.AddNode(noData), data2);

    // ACT
    var merged = graph1.MergeTransform(MakeCopy, NegateValue, graph2);

    // ASSERT
    Assert.That(merged.Edges.WithMarker(data1.Marker).Data.Value, Is.EqualTo(NegateValue(data1).Value));
    Assert.That(merged.Edges.WithMarker(data2.Marker).Data.Value, Is.EqualTo(NegateValue(data2).Value));
  }
}

internal class TestData
{
  internal int Value { get; init; }
  internal int Marker { get; init; }

  public override string ToString() => $"{Value}(m{Marker})";
}

internal static class TestDataExtensions
{
  internal static OldNode<,,> WithMarker(this IEnumerable<OldNode<,,>> nodes, int marker) =>
    nodes.First(node => node.Data.Marker == marker);

  internal static OldEdge<,,> WithMarker(this IEnumerable<OldEdge<,,>> edges, int marker) =>
    edges.First(edge => edge.Data.Marker == marker);

  internal static IEnumerable<TestData> Data(this IEnumerable<OldNode<,,>> nodes) => nodes.Select(node => node.Data);

  internal static IEnumerable<TestData> Data(this IEnumerable<OldEdge<,,>> edges) => edges.Select(edge => edge.Data);

  internal static IEnumerable<int> Markers(this IEnumerable<OldNode<,,>> nodes) =>
    nodes.Select(node => node.Data.Marker);

  internal static IEnumerable<int> Markers(this IEnumerable<OldEdge<,,>> edges) =>
    edges.Select(edge => edge.Data.Marker);
}