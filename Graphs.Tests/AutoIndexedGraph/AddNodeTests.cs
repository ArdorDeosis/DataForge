using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.AutoIndexedGraph;

internal class AddNodeTests
{
  [Test]
  public void AddNode_Node_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_Node_HasExpectedIndex()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(node.Index, Is.EqualTo(index));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedNode()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedIndex()
  {
    // ARRANGE
    const int index = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(index));

    // ACT
    graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(index));
    Assert.That(graph.Indices.Contains(index));
  }

  [Test]
  public void AddNode_InvalidIndex_ThrowsInvalidOperationException()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new StatelessIndexProvider<int, int>(data => data));
    graph.AddNode(data);

    // ASSERT
    Assert.That(() => graph.AddNode(data), Throws.InvalidOperationException);
  }

  [Test]
  public void TryAddNode_ValidIndex_ReturnsTrue()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ASSERT
    Assert.That(graph.TryAddNode(0, out _), Is.True);
  }

  [Test]
  public void TryAddNode_ValidIndex_OutputsNode()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

    // ACT
    graph.TryAddNode(data, out var node);

    // ASSERT
    Assert.That(node, Is.Not.Null);
    Assert.That(node!.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddNode_InvalidIndex_ReturnsFalse()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new AutoIndexedGraph<int, int, int>(new StatelessIndexProvider<int, int>(data => data));
    graph.AddNode(data);

    // ASSERT
    Assert.That(graph.TryAddNode(data, out _), Is.False);
  }
}