using DataForge.Graphs;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

internal class AddEdgeTests
{
  [Test]
  public void AddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    var edge = graph.AddEdge(origin.Index, destination.Index, data);

    // ASSERT
    Assert.That(edge.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);
    var edge = graph.AddEdge(origin.Index, destination.Index, 0);

    // ASSERT
    Assert.That(graph.Contains(edge));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void AddEdge_WrongNode_ThrowsArgumentException()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var validIndex = graph.AddNode(0).Index;
    var removedIndex = graph.AddNode(0).Index;
    graph.RemoveNode(removedIndex);
    const int invalidIndex = 0xC0FFEE;

    // ASSERT
    Assert.That(() => graph.AddEdge(validIndex, invalidIndex, 0), Throws.Exception);
    Assert.That(() => graph.AddEdge(validIndex, removedIndex, 0), Throws.Exception);
    Assert.That(() => graph.AddEdge(invalidIndex, validIndex, 0), Throws.Exception);
    Assert.That(() => graph.AddEdge(removedIndex, validIndex, 0), Throws.Exception);
  }

  [Test]
  public void TryAddEdge_ReturnsTrue()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.TryAddEdge(origin.Index, destination.Index, 0, out _));
  }

  [Test]
  public void TryAddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    graph.TryAddEdge(origin.Index, destination.Index, data, out var edge);

    // ASSERT
    Assert.That(edge, Is.Not.Null);
    Assert.That(edge.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    graph.TryAddEdge(origin.Index, destination.Index, 0, out var edge);

    // ASSERT
    Assert.That(graph.Contains(edge!));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void TryAddEdge_WrongNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var validIndex = graph.AddNode(0).Index;
    var removedIndex = graph.AddNode(0).Index;
    graph.RemoveNode(removedIndex);
    const int invalidIndex = 0xC0FFEE;

    // ASSERT
    Assert.That(graph.TryAddEdge(validIndex, invalidIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(validIndex, removedIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(invalidIndex, validIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(removedIndex, validIndex, 0, out _), Is.False);
  }
}