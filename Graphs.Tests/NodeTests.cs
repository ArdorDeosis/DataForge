using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.Graphs.Tests;

public class NodeTests
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
    Assert.That(node.Data, Is.EqualTo(NodeData));
  }

  [TestCaseSource(nameof(Nodes))]
  public void ValidNodes_NodeDataCanBeSet(INode<int, int> node)
  {
    // ARRANGE
    const int newData = 0xBEEF;
    node.Data = newData;

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(newData));
  }

  [TestCaseSource(nameof(InvalidNodes))]
  public void InvalidNode_DataIsAccessible(INode<int, int> node)
  {
    // ASSERT
    Assert.That(node.Data, Is.EqualTo(NodeData));
  }

  [TestCaseSource(nameof(InvalidNodes))]
  public void InvalidNode_DataIsImmutable(INode<int, int> node)
  {
    // ASSERT
    Assert.That(() => node.Data = 0, Throws.InvalidOperationException);
  }
}