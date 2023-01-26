using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.GraphComponents;

public class EdgeTests
{
  private const int EdgeData = 0xC0FFEE;

  private static IEnumerable<IEdge<int, int>> Edges()
  {
    var graph = new Graph<int, int>();
    yield return graph.AddEdge(graph.AddNode(0), graph.AddNode(0), EdgeData);

    var indexedGraph = new IndexedGraph<int, int, int>();
    indexedGraph.AddNode(0, 0);
    indexedGraph.AddNode(1, 1);
    yield return indexedGraph.AddEdge(0, 1, EdgeData);
  }

  private static IEnumerable<IEdge<int, int>> InvalidEdges()
  {
    foreach (var edge in Edges())
    {
      (edge as GraphComponent)?.Invalidate();
      yield return edge;
    }
  }

  [TestCaseSource(nameof(Edges))]
  public void EdgeHasExpectedData(IEdge<int, int> edge)
  {
    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(EdgeData));
  }

  [TestCaseSource(nameof(Edges))]
  public void ValidEdge_EdgeDataCanBeSet(IEdge<int, int> edge)
  {
    // ARRANGE
    const int newData = 0xBEEF;
    edge.Data = newData;

    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(newData));
  }

  [TestCaseSource(nameof(InvalidEdges))]
  public void InvalidEdge_DataIsAccessible(IEdge<int, int> edge)
  {
    // ASSERT
    Assert.That(edge.Data, Is.EqualTo(EdgeData));
  }

  [TestCaseSource(nameof(InvalidEdges))]
  public void InvalidEdge_DataIsImmutable(IEdge<int, int> edge)
  {
    // ASSERT
    Assert.That(() => edge.Data = 0, Throws.InvalidOperationException);
  }

  [TestCaseSource(nameof(InvalidEdges))]
  public void InvalidEdge_OriginIsNotAccessible(IEdge<int, int> edge)
  {
    // ASSERT
    Assert.That(() => edge.Origin, Throws.InvalidOperationException);
  }

  [TestCaseSource(nameof(InvalidEdges))]
  public void InvalidEdge_DestinationIsNotAccessible(IEdge<int, int> edge)
  {
    // ASSERT
    Assert.That(() => edge.Destination, Throws.InvalidOperationException);
  }
}