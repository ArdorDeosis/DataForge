using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GraphCreation.Tests;

internal class MultipartiteGraphCreationTests
{
  [Test]
  public void MultipartiteGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOptions<int, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSets = NodeDataSets,
      ShouldCreateEdge = (_, _, _) => false,
    };

    // ACT
    var graph = GraphCreator.MakeMultipartite(options);

    // ASSERT
    Assert.That(graph.Nodes.Select(node => node.Data), Is.EqualTo(NodeDataSets.SelectMany(set => set)));
  }

  [TestCaseSource(nameof(EdgeDirectionAndExpectedEdges))]
  public void MultipartiteGraph_HasExpectedStructureData(EdgeDirection edgeDirection, (int, int)[] expectedEdges)
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOptions<int, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSets = NodeDataSets,
      ShouldCreateEdge = (_, _, direction) => edgeDirection.HasFlag(direction),
    };

    // ACT
    var graph = GraphCreator.MakeMultipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)), Is.EquivalentTo(expectedEdges));
  }

  [TestCaseSource(nameof(EdgeDirectionAndExpectedEdges))]
  public void MultipartiteGraph_HasExpectedEdgeData(EdgeDirection edgeDirection, (int, int)[] expectedEdges)
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOptions<int, (int, int)>
    {
      CreateEdgeData = (from, to) => (from, to),
      NodeDataSets = NodeDataSets,
      ShouldCreateEdge = (_, _, direction) => edgeDirection.HasFlag(direction),
    };

    // ACT
    var graph = GraphCreator.MakeMultipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [Test]
  public void MultipartiteGraph_CustomEdgeCreation_HasExpectedEdges()
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOptions<string, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSets = new[]
      {
        new[] { "Horst", "Hermann" },
        new[] { "Asa", "Kriemhild" },
        new[] { "Þor", "Grima" },
      },
      ShouldCreateEdge = (fromData, toData, _) => fromData.Length < toData.Length,
    };
    var expectedEdges = new[]
    {
      ("Horst", "Kriemhild"),
      ("Hermann", "Kriemhild"),
      ("Asa", "Horst"), ("Asa", "Hermann"), ("Asa", "Grima"),
      ("Þor", "Horst"), ("Þor", "Hermann"), ("Þor", "Kriemhild"),
      ("Grima", "Hermann"), ("Grima", "Kriemhild"),
    };

    // ACT
    var graph = GraphCreator.MakeMultipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)), Is.EquivalentTo(expectedEdges));
  }

  private static int[][] NodeDataSets =>
    new[]
    {
      new[] { 0, 1 },
      new[] { 0xC0FFEE, 0xBEEF },
      new[] { int.MinValue, int.MaxValue },
    };

  private static (int, int)[] ForwardEdges =>
    new[]
    {
      (0, 0xC0FFEE), (0, 0xBEEF),
      (0, int.MinValue), (0, int.MaxValue),
      (1, 0xC0FFEE), (1, 0xBEEF),
      (1, int.MinValue), (1, int.MaxValue),
      (0xC0FFEE, int.MinValue), (0xC0FFEE, int.MaxValue),
      (0xBEEF, int.MinValue), (0xBEEF, int.MaxValue),
    };

  private static (int, int)[] BackwardEdges => ForwardEdges.Select(edge => (edge.Item2, edge.Item1)).ToArray();

  private static IEnumerable<object[]> EdgeDirectionAndExpectedEdges()
  {
    yield return new object[] { EdgeDirection.None, Array.Empty<(int, int)>() };
    yield return new object[] { EdgeDirection.Forward, ForwardEdges };
    yield return new object[] { EdgeDirection.Backward, BackwardEdges };
    yield return new object[] { EdgeDirection.ForwardAndBackward, ForwardEdges.Concat(BackwardEdges).ToArray() };
  }
}