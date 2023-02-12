using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.UnindexedGraph;

internal class CloneGraphTests
{
  [Test]
  public void Clone_DataIsEqual()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    graph.AddEdge(graph.AddNode(new { }), graph.AddNode(new { }), new { });

    // ACT
    var clonedGraph = graph.Clone();

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data),
      NUnit.Framework.Is.EquivalentTo(graph.Nodes.Select(node => node.Data)));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data),
      NUnit.Framework.Is.EquivalentTo(graph.Edges.Select(edge => edge.Data)));
  }

  [Test]
  public void Clone_StructureIsEqual()
  {
    // ARRANGE
    const string originNodeData = nameof(originNodeData);
    const string middleNodeData = nameof(middleNodeData);
    const string destinationNodeData = nameof(destinationNodeData);
    const string firstEdgeData = nameof(firstEdgeData);
    const string secondEdgeData = nameof(secondEdgeData);
    var graph = new Graph<string, string>();
    graph.AddEdge(graph.AddNode(originNodeData), graph.AddNode(middleNodeData), firstEdgeData);
    graph.AddEdge(graph.AddNode(middleNodeData), graph.AddNode(destinationNodeData), secondEdgeData);

    // ACT
    var clonedGraph = graph.Clone();

    // ASSERT
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is originNodeData &&
      edge.Destination.Data is middleNodeData &&
      edge.Data is firstEdgeData).ToList(), Has.Count.EqualTo(1));
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is middleNodeData &&
      edge.Destination.Data is destinationNodeData &&
      edge.Data is secondEdgeData).ToList(), Has.Count.EqualTo(1));
  }

  [Test]
  public void Clone_DataCloningFunctions_WorkAsExpected()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(1), graph.AddNode(2), 3);

    // ACT
    var clonedGraph = graph.Clone(data => -data, data => -data);

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data), NUnit.Framework.Is.EquivalentTo(new[] { -1, -2 }));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data), NUnit.Framework.Is.EquivalentTo(new[] { -3 }));
  }
}