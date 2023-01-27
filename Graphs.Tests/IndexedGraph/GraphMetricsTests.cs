using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

internal class GraphMetricsTests
{
  [Test]
  public void Order_HasExpectedValue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(0, 0);
    graph.AddNode(1, 0);
    graph.AddNode(2, 0);

    // ASSERT
    Assert.That(graph.Order, Is.EqualTo(3));
  }

  [Test]
  public void Size_HasExpectedValue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var node = graph.AddNode(0, 0);
    graph.AddEdge(0, 0, 0);
    graph.AddEdge(0, 0, 0);
    graph.AddEdge(0, 0, 0);

    // ASSERT
    Assert.That(graph.Size, Is.EqualTo(3));
  }
}