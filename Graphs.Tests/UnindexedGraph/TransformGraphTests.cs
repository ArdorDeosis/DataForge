using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

public class TransformGraphTests
{
  [Test]
  public void Transform_DataTransformationFunctions_WorkAsExpected()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(1), graph.AddNode(2), 3);

    // ACT
    var clonedGraph = graph.Transform(data => data.ToString(), data => data.ToString());

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { "1", "2" }));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(new[] { "3" }));
  }

  [Test]
  public void Transform_StructureIsEqual()
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
    var clonedGraph = graph.Transform(data => data, data => data);

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
}