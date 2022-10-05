using NUnit.Framework;

namespace Graph.Tests;

public partial class IndexedGraphTests
{
  [Test]
  public void Indices_ContainsExistingIndex()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.Indices, Does.Contain(1));
  }

  [Test]
  public void Indices_DoesNotContainRemovedIndex()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(1, new { });
    graph.RemoveNode(1);

    // ASSERT
    Assert.That(graph.Indices, Does.Not.Contain(1));
  }

  [Test]
  public void ContainsNode_NodeExists_True()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });

    // ASSERT
    Assert.That(graph.Contains(node));
  }

  [Test]
  public void ContainsNode_NodeDoesNotExist_False()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var node = graph.AddNode(1, new { });
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
  }

  [Test]
  public void ContainsEdge_EdgeExists_True()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var edge = graph.AddEdge(
      graph.AddNode(1, new { }),
      graph.AddNode(2, new { }),
      new { }
    );

    // ASSERT
    Assert.That(graph.Contains(edge));
  }

  [Test]
  public void ContainsEdge_EdgeDoesNotExist_False()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    var edge = graph.AddEdge(
      graph.AddNode(1, new { }),
      graph.AddNode(2, new { }),
      new { }
    );
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
  }

  [Test]
  public void ContainsIndex_IndexExists_True()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();
    graph.AddNode(0xC0FFEE, new { });

    // ASSERT
    Assert.That(graph.Contains(0xC0FFEE));
  }

  [Test]
  public void ContainsIndex_IndexDoesNotExist_False()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, object, object>();

    // ASSERT
    Assert.That(graph.Contains(0xC0FFEE), Is.False);
  }
}