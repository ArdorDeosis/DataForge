using DataForge.Graphs;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

internal class GetNodeTests
{
  [Test]
  public void Indexer_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph[index], Is.EqualTo(node));
  }

  [Test]
  public void GetNode_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph.GetNode(index), Is.EqualTo(node));
  }

  [Test]
  public void GetNodeOrNull_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph.GetNodeOrNull(index), Is.EqualTo(node));
  }

  [Test]
  public void TryGetNode_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(graph.TryGetNode(index, out _), Is.True);
  }

  [Test]
  public void TryGetNode_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));
    var node = graph.AddNode(default);

    // ACT
    graph.TryGetNode(index, out var returnedNode);

    // ASSERT
    Assert.That(returnedNode, Is.EqualTo(node));
  }

  [Test]
  public void Indexer_NotExistingNode_ThrowsKeyNotFound()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(default));

    // ASSERT
    Assert.That(() => graph[0], Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNode_NotExistingNode_ThrowsKeyNotFound()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(() => graph.GetNode(0), Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNodeOrNull_NotExistingNode_ReturnsNull()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.GetNodeOrNull(0), Is.Null);
  }

  [Test]
  public void TryGetNode_NotExistingNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.TryGetNode(0, out _), Is.False);
  }
}