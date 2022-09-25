using System.Collections.Generic;
using NUnit.Framework;

namespace Graph.Tests;

public class IndexedGraphEdgeHandlingTests
{
  [Test]
  public void AddEdge_WithReferences_EdgeIsInGraph()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.That(graph.Edges, Contains.Item(edge));
  }

  [Test]
  public void AddEdge_WithIndices_EdgeIsInGraph()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(1, new { });
    graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(1, 2, new { });

    // ASSERT
    Assert.That(graph.Edges, Contains.Item(edge));
  }

  [Test]
  public void AddEdge_WithReferences_EdgeHasData()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });
    var data = new { };

    // ACT
    var edge = graph.AddEdge(startNode, endNode, data);

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_WithIndices_EdgeHasData()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(1, new { });
    graph.AddNode(2, new { });
    var data = new { };

    // ACT
    var edge = graph.AddEdge(1, 2, data);

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_WithReferences_EdgeHasReferenceToNodes()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(edge.Start, Is.EqualTo(startNode));
      Assert.That(edge.End, Is.EqualTo(endNode));
    });
  }

  [Test]
  public void AddEdge_WithIndices_EdgeHasReferenceToNodes()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(1, 2, new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(edge.Start, Is.EqualTo(startNode));
      Assert.That(edge.End, Is.EqualTo(endNode));
    });
  }

  [Test]
  public void AddEdge_WithReferences_NodesHaveReferenceToEdge()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(startNode.OutgoingEdges, Contains.Item(edge));
      Assert.That(startNode.Edges, Contains.Item(edge));
      Assert.That(endNode.IncomingEdges, Contains.Item(edge));
      Assert.That(endNode.Edges, Contains.Item(edge));
    });
  }

  [Test]
  public void AddEdge_WithIndices_NodesHaveReferenceToEdge()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });

    // ACT
    var edge = graph.AddEdge(1, 2, new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(startNode.OutgoingEdges, Contains.Item(edge));
      Assert.That(startNode.Edges, Contains.Item(edge));
      Assert.That(endNode.IncomingEdges, Contains.Item(edge));
      Assert.That(endNode.Edges, Contains.Item(edge));
    });
  }

  [Test]
  public void AddEdge_WithInvalidNode_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var validNode = graph.AddNode(1, new { });
    var invalidNode = graph.AddNode(2, new { });
    invalidNode.Invalidate();

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(() => graph.AddEdge(validNode, invalidNode, new { }), Throws.InvalidOperationException);
      Assert.That(() => graph.AddEdge(invalidNode, validNode, new { }), Throws.InvalidOperationException);
    });
  }

  [Test]
  public void AddEdge_WithInvalidIndex_ThrowsKeyNotFoundException()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(1, new { });

    // ASSERT
    Assert.That(() => graph.AddEdge(1, 2, new { }), Throws.Exception.TypeOf<KeyNotFoundException>());
    Assert.That(() => graph.AddEdge(2, 1, new { }), Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void AddEdge_WithNodeInOtherGraph_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var otherGraph = new IndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });
    var otherNode = otherGraph.AddNode(2, new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(() => graph.AddEdge(node, otherNode, new { }), Throws.ArgumentException);
      Assert.That(() => graph.AddEdge(otherNode, node, new { }), Throws.ArgumentException);
    });
  }

  [Test]
  public void RemoveEdge_EdgeIsNotInGraph()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge));
  }

  [Test]
  public void RemoveEdge_NodesHaveNoReferenceToEdge()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(startNode.Edges, Does.Not.Contain(edge));
      Assert.That(endNode.Edges, Does.Not.Contain(edge));
    });
  }

  [Test]
  public void RemoveEdge_ValidEdge_ReturnsTrue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.That(graph.RemoveEdge(edge));
  }

  [Test]
  public void RemoveEdge_EdgeFromDifferentGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var otherGraph = new IndexedGraph<int, object, object>();
    var edge = otherGraph.AddEdge(
      otherGraph.AddNode(1, new { }),
      otherGraph.AddNode(2, new { }),
      new { }
    );

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.False);
  }

  [Test]
  public void RemoveEdges_ByPredicate_MatchingEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, bool>();
    var startNode = graph.AddNode(1, new { });
    var endNode = graph.AddNode(2, new { });
    var edgeToRemove = graph.AddEdge(startNode, endNode, true);
    var edgeToKeep = graph.AddEdge(endNode, startNode, false);

    // ACT
    graph.RemoveEdges(edge => edge.Data);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Edges, Does.Not.Contain(edgeToRemove));
      Assert.That(graph.Edges, Does.Contain(edgeToKeep));
    });
  }
}