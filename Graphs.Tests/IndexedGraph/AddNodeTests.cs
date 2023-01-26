using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

[TestFixture]
public class AddNodeTests
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
}