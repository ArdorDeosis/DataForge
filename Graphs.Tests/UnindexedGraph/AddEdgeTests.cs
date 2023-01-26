using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

public class AddEdgeTests
{
  [Test]
  public void AddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    var edge = graph.AddEdge(origin, destination, data);

    // ASSERT
    Assert.That(edge.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ASSERT
    Assert.That(graph.Contains(edge));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void AddEdge_WrongNode_ThrowsArgumentException()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var validNode = graph.AddNode(0);
    var invalidNode = new Graph<int, int>().AddNode(0);
    var removedNode = graph.AddNode(0);
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(() => graph.AddEdge(validNode, invalidNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(validNode, removedNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(invalidNode, validNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(removedNode, validNode, 0), Throws.ArgumentException);
  }

  [Test]
  public void TryAddEdge_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.TryAddEdge(origin, destination, 0, out _));
  }

  [Test]
  public void TryAddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    graph.TryAddEdge(origin, destination, data, out var edge);

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
    var graph = new Graph<int, int>();

    // ACT
    graph.TryAddEdge(graph.AddNode(0), graph.AddNode(0), 0, out var edge);

    // ASSERT
    Assert.That(graph.Contains(edge!));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void TryAddEdge_WrongNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var validNode = graph.AddNode(0);
    var invalidNode = new Graph<int, int>().AddNode(0);
    var removedNode = graph.AddNode(0);
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(graph.TryAddEdge(validNode, invalidNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(validNode, removedNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(invalidNode, validNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(removedNode, validNode, 0, out _), Is.False);
  }
}