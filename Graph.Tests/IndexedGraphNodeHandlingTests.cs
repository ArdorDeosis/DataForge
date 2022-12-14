using System.Collections.Generic;
using NUnit.Framework;

namespace Graph.Tests;

public class IndexedGraphNodeHandlingTests
{
  [Test]
  public void AddNode_NodeIsInGraph()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.Nodes, Contains.Item(node));
  }

  [Test]
  public void AddNode_NodeHasData()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var data = new { };

    // ACT
    var node = graph.AddNode(1, data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_IndexAlreadyExists_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    graph.AddNode(1, new { });

    // ACT + ASSERT
    Assert.That(() => graph.AddNode(1, new { }), Throws.InvalidOperationException);
  }

  [Test]
  public void Indexer_NodeIsInGraph_GetsNode()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph[1], Is.EqualTo(node));
  }

  [Test]
  public void Indexer_NodeIsNotInGraph_ThrowsKeyNotFoundException()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT + ASSERT
    Assert.That(() => graph[1], Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNode_NodeIsInGraph_GetsNode()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.GetNode(1), Is.EqualTo(node));
  }

  [Test]
  public void GetNode_NodeIsNotInGraph_ThrowsKeyNotFoundException()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT + ASSERT
    Assert.That(() => graph.GetNode(1), Throws.Exception.TypeOf<KeyNotFoundException>());
  }


  [Test]
  public void TryGetNode_NodeIsInGraph_GetsNodeAndReturnsTrue()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.TryGetNode(1, out var retrievedNode));
    Assert.That(retrievedNode, Is.EqualTo(node));
  }

  [Test]
  public void TryGetNode_NodeIsNotInGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ASSERT
    Assert.That(graph.TryGetNode(1, out _), Is.False);
  }

  [Test]
  public void GetIndexOf_NodeIsInGraph_GetsIndex()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.GetIndexOf(node), Is.EqualTo(1));
  }

  [Test]
  public void GetIndexOf_NodeIsNotInGraphOrNull_ThrowsInvalidOperationException()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = new OldIndexedGraph<int, object, object>().AddNode(1, new { });

    // ACT + ASSERT
    Assert.That(() => graph.GetIndexOf(null!), Throws.InvalidOperationException);
    Assert.That(() => graph.GetIndexOf(node), Throws.InvalidOperationException);
  }


  [Test]
  public void TryGetIndexOf_NodeIsInGraph_GetsNodeAndReturnsTrue()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ACT
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.TryGetIndexOf(node, out var index));
    Assert.That(index, Is.EqualTo(1));
  }

  [Test]
  public void TryGetIndexOf_NodeIsNotInGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = new OldIndexedGraph<int, object, object>().AddNode(1, new { });

    // ASSERT
    Assert.That(graph.TryGetIndexOf(node, out _), Is.False);
    Assert.That(graph.TryGetIndexOf(null!, out _), Is.False);
  }

  [Test]
  public void RemoveNodeByReference_ValidNode_ReturnsTrue()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.RemoveNode(node));
  }

  [Test]
  public void RemoveNodeByIndex_ValidNode_ReturnsTrue()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.RemoveNode(1));
  }

  [Test]
  public void RemoveNodeByReference_NodeFromOtherGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = new OldIndexedGraph<int, object, object>().AddNode(1, new { });

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.False);
  }

  [Test]
  public void RemoveNodeByIndex_IndexDoesNotExist_ReturnsFalse()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();

    // ASSERT
    Assert.That(graph.RemoveNode(1), Is.False);
  }

  [Test]
  public void RemoveNodeByReference_NodeIsNotInGraph()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Nodes, Does.Not.Contain(node));
  }

  [Test]
  public void RemoveNodeByIndex_NodeIsNotInGraph()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });

    // ACT
    graph.RemoveNode(1);

    // ASSERT
    Assert.That(graph.Nodes, Does.Not.Contain(node));
  }

  [Test]
  public void RemoveNodeByReference_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var middleNode = graph.AddNode(2, new { });
    var endNode = graph.AddNode(3, new { });
    var edgeToMiddleNode = graph.AddEdge(startNode, middleNode, new { });
    var edgeFromMiddleNode = graph.AddEdge(middleNode, endNode, new { });

    // ACT
    graph.RemoveNode(middleNode);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Edges, Does.Not.Contain(edgeToMiddleNode));
      Assert.That(graph.Edges, Does.Not.Contain(edgeFromMiddleNode));
    });
  }

  [Test]
  public void RemoveNodeByIndex_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var middleNode = graph.AddNode(2, new { });
    var endNode = graph.AddNode(3, new { });
    var edgeToMiddleNode = graph.AddEdge(startNode, middleNode, new { });
    var edgeFromMiddleNode = graph.AddEdge(middleNode, endNode, new { });

    // ACT
    graph.RemoveNode(2);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Edges, Does.Not.Contain(edgeToMiddleNode));
      Assert.That(graph.Edges, Does.Not.Contain(edgeFromMiddleNode));
    });
  }

  [Test]
  public void RemoveNodeByReference_ConnectedNodesHaveNoReferenceToImplicitlyRemovedEdges()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var middleNode = graph.AddNode(2, new { });
    var endNode = graph.AddNode(3, new { });
    var edgeToMiddleNode = graph.AddEdge(startNode, middleNode, new { });
    var edgeFromMiddleNode = graph.AddEdge(middleNode, endNode, new { });

    // ACT
    graph.RemoveNode(middleNode);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(startNode.Edges, Does.Not.Contain(edgeToMiddleNode));
      Assert.That(endNode.Edges, Does.Not.Contain(edgeFromMiddleNode));
    });
  }

  [Test]
  public void RemoveNodeByIndex_ConnectedNodesHaveNoReferenceToImplicitlyRemovedEdges()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, object, object>();
    var startNode = graph.AddNode(1, new { });
    var middleNode = graph.AddNode(2, new { });
    var endNode = graph.AddNode(3, new { });
    var edgeToMiddleNode = graph.AddEdge(startNode, middleNode, new { });
    var edgeFromMiddleNode = graph.AddEdge(middleNode, endNode, new { });

    // ACT
    graph.RemoveNode(2);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(startNode.Edges, Does.Not.Contain(edgeToMiddleNode));
      Assert.That(endNode.Edges, Does.Not.Contain(edgeFromMiddleNode));
    });
  }

  [Test]
  public void RemoveNodes_ByPredicate_NodesAreNotInGraph()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, bool, object>();
    var nodeToRemove = graph.AddNode(1, true);
    var nodeToKeep = graph.AddNode(2, false);

    // ACT
    graph.RemoveNodes(node => node.Data);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Nodes, Does.Contain(nodeToKeep));
      Assert.That(graph.Nodes, Does.Not.Contain(nodeToRemove));
    });
  }

  [Test]
  public void RemoveNodes_ByPredicateIncludingIndex_NodesAreNotInGraph()
  {
    // ARRANGE
    var graph = new OldIndexedGraph<int, bool, object>();
    var nodeToRemove = graph.AddNode(1, true);
    var nodeToKeep = graph.AddNode(2, false);

    // ACT
    graph.RemoveNodes((_, index) => index < 2);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Nodes, Does.Contain(nodeToKeep));
      Assert.That(graph.Nodes, Does.Not.Contain(nodeToRemove));
    });
  }
}