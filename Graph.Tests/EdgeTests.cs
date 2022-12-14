using NUnit.Framework;

namespace Graph.Tests;

public class EdgeTests
{
  [Test]
  public void EdgeConstructor_NodeFromDifferentGraph_ThrowsArgumentException()
  {
    // ARRANGE
    var graph = new OldGraph<object, object>();
    var nodeInSameGraph = graph.AddNode(new { });
    var nodeInOtherGraph = new OldGraph<object, object>().AddNode(new { });

    // ASSERT
    Assert.Multiple(() =>
    {
      Assert.That(() => new OldEdge<,,>(graph, nodeInSameGraph, nodeInOtherGraph, new { }),
        Throws.ArgumentException);
      Assert.That(() => new OldEdge<,,>(graph, nodeInOtherGraph, nodeInSameGraph, new { }),
        Throws.ArgumentException);
    });
  }

  [Test]
  public void Nodes_ContainsStartAndEndNode()
  {
    // ARRANGE
    var graph = new OldGraph<object, object>();
    var startNode = graph.AddNode(new { });
    var endNode = graph.AddNode(new { });
    var expectedNodes = new[] { startNode, endNode };

    // ACT
    var edge = graph.AddEdge(startNode, endNode, new { });

    // ASSERT
    Assert.That(edge.Nodes, Is.EquivalentTo(expectedNodes));
  }
}