using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class GraphMetricsTests
{
  [Test]
  public void Order_HasExpectedValue()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
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
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var node = graph.AddNode(0);
    graph.AddEdge(0, 0, 0);
    graph.AddEdge(0, 0, 0);
    graph.AddEdge(0, 0, 0);

    // ASSERT
    Assert.That(graph.Size, Is.EqualTo(3));
  }
}