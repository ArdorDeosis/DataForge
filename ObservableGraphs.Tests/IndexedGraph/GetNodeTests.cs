using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.IndexedGraph;

internal class GetNodeTests
{
  [Test]
  public void Indexer_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(graph[index], Is.EqualTo(node));
  }

  [Test]
  public void GetNode_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(graph.GetNode(index), Is.EqualTo(node));
  }

  [Test]
  public void GetNodeOrNull_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(graph.GetNodeOrNull(index), Is.EqualTo(node));
  }

  [Test]
  public void TryGetNode_ExistingNode_ReturnsTrue()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(graph.TryGetNode(index, out _), Is.True);
  }

  [Test]
  public void TryGetNode_ExistingNode_IsReturned()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var node = graph.AddNode(index, 0);

    // ACT
    graph.TryGetNode(index, out var returnedNode);

    // ASSERT
    Assert.That(returnedNode, Is.EqualTo(node));
  }

  [Test]
  public void Indexer_NotExistingNode_ThrowsKeyNotFound()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(() => graph[index], Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNode_NotExistingNode_ThrowsKeyNotFound()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(() => graph.GetNode(index), Throws.Exception.TypeOf<KeyNotFoundException>());
  }

  [Test]
  public void GetNodeOrNull_NotExistingNode_ReturnsNull()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.GetNodeOrNull(index), Is.Null);
  }

  [Test]
  public void TryGetNode_NotExistingNode_ReturnsFalse()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new ObservableIndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(graph.TryGetNode(index, out _), Is.False);
  }
}