using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

internal class AddNodeTests
{
  [Test]
  public void AddNode_Node_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();

    // ACT
    var node = graph.AddNode(0, data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_Node_HasExpectedIndex()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();

    // ACT
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(node.Index, Is.EqualTo(index));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedNode()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();

    // ACT
    var node = graph.AddNode(0, 0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedIndex()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();

    // ACT
    graph.AddNode(index, 0);

    // ASSERT
    Assert.That(graph.Contains(index));
    Assert.That(graph.Indices.Contains(index));
  }

  [Test]
  public void AddNode_InvalidIndex_ThrowsInvalidOperationException()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(index, 0);

    // ASSERT
    Assert.That(() => graph.AddNode(index, 0), Throws.InvalidOperationException);
  }

  [Test]
  public void TryAddNode_ValidIndex_ReturnsTrue()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();

    // ASSERT
    Assert.That(() => graph.TryAddNode(index, 0, out _), Is.True);
  }

  [Test]
  public void TryAddNode_ValidIndex_OutputsNode()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    const int data = 0xBEEF;
    var graph = new IndexedGraph<int, int, int>();

    // ACT
    graph.TryAddNode(index, data, out var node);

    // ASSERT
    Assert.That(node, Is.Not.Null);
    Assert.That(node!.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddNode_InvalidIndex_ReturnsFalse()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new IndexedGraph<int, int, int>();
    graph.AddNode(index, 0);

    // ASSERT
    Assert.That(() => graph.TryAddNode(index, 0, out _), Is.False);
  }
}