using NUnit.Framework;

namespace Graph.Tests;

public class NodeTests
{
  [Test]
  public void Predecessors_CorrectNodes()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var middleNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    graph.AddEdge(startNode, middleNode, new { });
    graph.AddEdge(middleNode, endNode, new { });

    // ASSERT
    Assert.That(middleNode.Predecessors, Is.EquivalentTo(new[] { startNode }));
  }

  [Test]
  public void Successors_CorrectNodes()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var middleNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    graph.AddEdge(startNode, middleNode, new { });
    graph.AddEdge(middleNode, endNode, new { });

    // ASSERT
    Assert.That(middleNode.Successors, Is.EquivalentTo(new[] { endNode }));
  }

  [Test]
  public void Neighbours_CorrectNodes()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var startNode = graph.AddNode(new { });
    var middleNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    graph.AddEdge(startNode, middleNode, new { });
    graph.AddEdge(middleNode, endNode, new { });

    // ASSERT
    Assert.That(middleNode.Neighbours, Is.EquivalentTo(new[]
    {
      endNode, 
      startNode,
    }));
  }
}