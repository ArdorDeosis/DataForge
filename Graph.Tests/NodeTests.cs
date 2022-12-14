using System.Linq;
using NUnit.Framework;

namespace Graph.Tests;

public class NodeTests
{
  [Test]
  public void Predecessors_CorrectNodes()
  {
    // ARRANGE
    var graph = new OldGraph<object, object>();
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
    var graph = new OldGraph<object, object>();
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
    var graph = new OldGraph<object, object>();
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

  [Test]
  public void Degree_CorrectValue()
  {
    // ARRANGE
    var graph = new OldGraph<int, int>();
    var nodes = graph.AddNodes(Enumerable.Range(0, 4)).ToArray();
    graph.AddEdge(nodes[1], nodes[2], 0);
    graph.AddEdge(nodes[2], nodes[3], 0);
    graph.AddEdge(nodes[3], nodes[3], 0);

    var expectedDegrees = new[] { 0, 1, 2, 3 };

    // ASSERT
    for (var i = 0; i < 4; i++)
      Assert.That(nodes[i].Degree, Is.EqualTo(expectedDegrees[i]));
  }
}