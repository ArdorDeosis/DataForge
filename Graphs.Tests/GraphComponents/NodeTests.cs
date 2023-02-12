using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.GraphComponents;

internal class NodeTests
{
  private const int NodeData = 0xC0FFEE;

  private static IEnumerable<INode<int, int>> Nodes()
  {
    yield return new Graph<int, int>().AddNode(NodeData);
    yield return new IndexedGraph<int, int, int>().AddNode(0, NodeData);
  }

  private static IEnumerable<INode<int, int>> InvalidNodes()
  {
    foreach (var node in Nodes())
    {
      (node as GraphComponent)?.Invalidate();
      yield return node;
    }
  }

  [TestCaseSource(nameof(Nodes))]
  public void NodeHasExpectedData(INode<int, int> node)
  {
    // ASSERT
    Assert.That(node.Data, NUnit.Framework.Is.EqualTo(NodeData));
  }

  [TestCaseSource(nameof(Nodes))]
  public void ValidNodes_NodeDataCanBeSet(INode<int, int> node)
  {
    // ARRANGE
    const int newData = 0xBEEF;
    node.Data = newData;

    // ASSERT
    Assert.That(node.Data, NUnit.Framework.Is.EqualTo(newData));
  }

  [TestCaseSource(nameof(InvalidNodes))]
  public void InvalidNode_DataIsAccessible(INode<int, int> node)
  {
    // ASSERT
    Assert.That(node.Data, NUnit.Framework.Is.EqualTo(NodeData));
  }

  [TestCaseSource(nameof(InvalidNodes))]
  public void InvalidNode_DataIsImmutable(INode<int, int> node)
  {
    // ASSERT
    Assert.That(() => node.Data = 0, Throws.InvalidOperationException);
  }

  [Test]
  public void IndexedNode_IndexIsCorrect()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(node.Index, NUnit.Framework.Is.EqualTo(index));
  }

  [Test]
  public void InvalidIndexedNode_GettingIndex_Throws()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(() => node.Index, Throws.Exception);
  }

  [Test]
  public void IndexedNode_TryGetIndex_ReturnsTrue()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);

    // ASSERT
    Assert.That(node.TryGetIndex(out _), NUnit.Framework.Is.True);
  }

  [Test]
  public void IndexedNode_TryGetIndex_OutputsIndex()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);

    // ACT
    node.TryGetIndex(out var retrievedIndex);

    // ASSERT
    Assert.That(retrievedIndex, NUnit.Framework.Is.EqualTo(index));
  }

  [Test]
  public void InvalidIndexedNode_TryGetIndex_ReturnsFalse()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index = 0xC0FFEE;
    var node = graph.AddNode(index, 0);
    graph.RemoveNode(index);

    // ASSERT
    Assert.That(node.TryGetIndex(out _), NUnit.Framework.Is.False);
  }
}