using NUnit.Framework;

namespace Graph.Tests;

public class GraphEdgeHandlingTests
{
  [Test]
  public void AddEdge_EdgeIsInGraph()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });

    // ACT
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.That(graph.Edges, Contains.Item(edge));
  }

  [Test]
  public void AddEdge_EdgeHasData()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    var data = new { };

    // ACT
    var edge = graph.AddEdge(startNode, endNode, data);

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_EdgeHasReferenceToNodes()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });

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
  public void AddEdge_NodesHaveReferenceToEdge()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });

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
  public void AddEdge_WithInvalidNode_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var validNode = graph.AddNode(new { });
    var invalidNode = graph.AddNode(new { });
    invalidNode.Invalidate();

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(() => graph.AddEdge(validNode, invalidNode, new { }), Throws.InvalidOperationException);
      Assert.That(() => graph.AddEdge(invalidNode, validNode, new { }), Throws.InvalidOperationException);
    });
  }

  [Test]
  public void AddEdge_WithNodeInOtherGraph_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var otherGraph = new Graph<object, object>();
    var node = graph.AddNode(new { });
    var otherNode = otherGraph.AddNode(new { });

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
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
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
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
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
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.That(graph.RemoveEdge(edge));
  }

  [Test]
  public void RemoveEdge_EdgeFromDifferentGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var otherGraph = new Graph<object, object>();
    var edge = otherGraph.AddEdge(
      otherGraph.AddNode(new { }),
      otherGraph.AddNode(new { }),
      new { }
    );

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.False);
  }

  [Test]
  public void RemoveEdges_ByPredicate_MatchingEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<object, bool>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
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