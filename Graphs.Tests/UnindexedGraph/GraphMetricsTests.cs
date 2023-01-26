using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

public class GraphMetricsTests
{
  [Test]
  public void Order_HasExpectedValue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddNode(0);
    graph.AddNode(0);
    graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Order, Is.EqualTo(3));
  }

  [Test]
  public void Size_HasExpectedValue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);
    graph.AddEdge(node, node, 0);
    graph.AddEdge(node, node, 0);
    graph.AddEdge(node, node, 0);

    // ASSERT
    Assert.That(graph.Size, Is.EqualTo(3));
  }
}