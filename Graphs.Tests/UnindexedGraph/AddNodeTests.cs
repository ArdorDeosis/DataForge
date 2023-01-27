using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

internal class AddNodeTests
{
  [Test]
  public void AddNode_Node_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedNode()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }

  [Test]
  public void AddNodes_Nodes_HaveExpectedData()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();

    // ACT
    var nodes = graph.AddNodes(data).ToList();

    // ASSERT
    Assert.That(nodes, Has.Count.EqualTo(data.Length));
    Assert.That(nodes.Select(node => node.Data), Is.EquivalentTo(data));
  }

  [Test]
  public void AddNodes_Graph_ContainsAddedNodes()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }
}