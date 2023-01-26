using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

[TestFixture]
public class AddEdgeTests
{
  [Test]
  public void AddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();
    var origin = graph.AddNode(0, 0);
    var destination = graph.AddNode(1, 1);

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
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(0, 0);
    graph.AddNode(1, 1);
    var edge = graph.AddEdge(0, 1, 0);

    // ASSERT
    Assert.That(graph.Contains(edge));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void AddEdge_WrongNode_ThrowsArgumentException()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int validIndex = 0xC0FFEE;
    const int removedIndex = 0xBEEF;
    const int invalidIndex = 0xF00D;
    graph.AddNode(validIndex, validIndex);
    graph.AddNode(1, 1);
    graph.RemoveNode(removedIndex);

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
    var graph = new IndexedGraph<int, int, int>();
    var origin = graph.AddNode(0, 0);
    var destination = graph.AddNode(1, 1);

    // ASSERT
    Assert.That(graph.TryAddEdge(origin.Index, destination.Index, 0, out _));
  }

  [Test]
  public void TryAddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();
    var origin = graph.AddNode(0, 0);
    var destination = graph.AddNode(1, 1);

    // ACT
    graph.TryAddEdge(origin.Index, destination.Index, data, out var edge);

    // ASSERT
    Assert.That(edge, Is.Not.Null);
    Assert.That(edge!.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(0, 0);
    graph.AddNode(1, 1);

    // ACT
    graph.TryAddEdge(0, 1, 0, out var edge);

    // ASSERT
    Assert.That(graph.Contains(edge!));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void TryAddEdge_WrongNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int validIndex = 0xC0FFEE;
    const int removedIndex = 0xBEEF;
    const int invalidIndex = 0xF00D;
    graph.AddNode(validIndex, validIndex);
    graph.AddNode(1, 1);
    graph.RemoveNode(removedIndex);

    // ASSERT
    Assert.That(graph.TryAddEdge(validIndex, invalidIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(validIndex, removedIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(invalidIndex, validIndex, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(removedIndex, validIndex, 0, out _), Is.False);
  }
}