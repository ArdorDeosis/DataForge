using NUnit.Framework;

namespace Graph.Tests;

public class GraphTests
{
  [Test]
  public void ContainsNode_NodeExists_True()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var node = graph.AddNode(new { });

    // ASSERT
    Assert.That(graph.Contains(node));
  }

  [Test]
  public void ContainsNode_NodeDoesNotExist_False()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var node = graph.AddNode(new { });
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
  }

  [Test]
  public void ContainsEdge_EdgeExists_True()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var edge = graph.AddEdge(
      graph.AddNode(new { }),
      graph.AddNode(new { }),
      new { }
    );

    // ASSERT
    Assert.That(graph.Contains(edge));
  }

  [Test]
  public void ContainsEdge_EdgeDoesNotExist_False()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    var edge = graph.AddEdge(
      graph.AddNode(new { }),
      graph.AddNode(new { }),
      new { }
    );
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
  }
}