using System.Linq;
using NUnit.Framework;

namespace Graph.Tests;

public class GraphNodeHandlingTests
{
  [Test]
  public void AddNode_NodeIsInGraph()
  {
    // ARRANGE
    var graph = new Graph<object, object>();

    // ACT
    var node = graph.AddNode(new { });

    // ASSERT
    Assert.That(graph.Nodes, Contains.Item(node));
  }

  [Test]
  public void AddNode_NodeHasData()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var data = new { };

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNodes_Array_NodesAreInGraph()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    object[] nodeData = { new { }, new { }, new { } };

    // ACT
    var nodes = graph.AddNodes(nodeData);

    // ASSERT
    Assert.That(graph.Nodes, Is.EquivalentTo(nodes));
  }

  [Test]
  public void AddNodes_Enumerable_NodesAreInGraph()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    object[] nodeData = { new { }, new { }, new { } };

    // ACT
    var nodes = graph.AddNodes(nodeData.AsEnumerable());

    // ASSERT
    Assert.That(graph.Nodes, Is.EquivalentTo(nodes));
  }

  [Test]
  public void RemoveNode_ValidNode_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var node = graph.AddNode(new { });

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.True);
  }

  [Test]
  public void RemoveNode_NodeFromOtherGraph_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var node = new Graph<object, object>().AddNode(new { });

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.False);
  }

  [Test]
  public void RemoveNode_NodeIsNotInGraph()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var node = graph.AddNode(new { });

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Nodes, Does.Not.Contain(node));
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var middleNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
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
  public void RemoveNode_ConnectedNodesHaveNoReferenceToImplicitlyRemovedEdges()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var middleNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
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
  public void RemoveNodes_ByPredicate_NodesAreNotInGraph()
  {
    // ARRANGE
    var graph = new Graph<bool, object>();
    var nodeToRemove = graph.AddNode(true);
    var nodeToKeep = graph.AddNode(false);

    // ACT
    graph.RemoveNodes(node => node.Data);

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(graph.Nodes, Does.Contain(nodeToKeep));
      Assert.That(graph.Nodes, Does.Not.Contain(nodeToRemove));
    });
  }
}