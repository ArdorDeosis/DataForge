using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Bipartite;

internal class BipartiteGraphCreationTests
{
  private static IEnumerable<int> NodeDataSetA => new[] { 0xC0FFEE, 0xBEEF };
  private static IEnumerable<int> NodeDataSetB => new[] { int.MinValue, int.MaxValue };

  private static (int, int)[] ForwardEdges =>
    new[]
    {
      (0xC0FFEE, int.MinValue), (0xC0FFEE, int.MaxValue),
      (0xBEEF, int.MinValue), (0xBEEF, int.MaxValue),
    };

  private static (int, int)[] BackwardEdges => ForwardEdges.Select(edge => (edge.Item2, edge.Item1)).ToArray();

  [Test]
  public void BipartiteGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOptions<int, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSetA = NodeDataSetA,
      NodeDataSetB = NodeDataSetB,
      ShouldCreateEdge = (_, _, _) => false,
    };

    // ACT
    var graph = GraphCreator.MakeBipartite(options);

    // ASSERT
    Assert.That(graph.Nodes.Select(node => node.Data), Is.EqualTo(NodeDataSetA.Concat(NodeDataSetB)));
  }

  [TestCaseSource(nameof(EdgeDirectionAndExpectedEdges))]
  public void BipartiteGraph_HasExpectedStructureData(EdgeDirection edgeDirection, (int, int)[] expectedEdges)
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOptions<int, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSetA = NodeDataSetA,
      NodeDataSetB = NodeDataSetB,
      ShouldCreateEdge = (_, _, direction) => edgeDirection.HasFlag(direction),
    };

    // ACT
    var graph = GraphCreator.MakeBipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)), Is.EquivalentTo(expectedEdges));
  }

  [TestCaseSource(nameof(EdgeDirectionAndExpectedEdges))]
  public void BipartiteGraph_HasExpectedEdgeData(EdgeDirection edgeDirection, (int, int)[] expectedEdges)
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOptions<int, (int, int)>
    {
      CreateEdgeData = (from, to) => (from, to),
      NodeDataSetA = NodeDataSetA,
      NodeDataSetB = NodeDataSetB,
      ShouldCreateEdge = (_, _, direction) => edgeDirection.HasFlag(direction),
    };

    // ACT
    var graph = GraphCreator.MakeBipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [Test]
  public void BipartiteGraph_CustomEdgeCreation_HasExpectedEdges()
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOptions<string, int>
    {
      CreateEdgeData = (_, _) => 0,
      NodeDataSetA = new[] { "Horst", "Hermann" },
      NodeDataSetB = new[] { "Asa", "Kriemhild" },
      ShouldCreateEdge = (fromData, toData, _) => fromData.Length < toData.Length,
    };

    var expectedEdges = new[]
    {
      ("Horst", "Kriemhild"), ("Hermann", "Kriemhild"),
      ("Asa", "Horst"), ("Asa", "Hermann"),
    };

    // ACT
    var graph = GraphCreator.MakeBipartite(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)), Is.EquivalentTo(expectedEdges));
  }

  private static IEnumerable<object[]> EdgeDirectionAndExpectedEdges()
  {
    yield return new object[] { EdgeDirection.None, Array.Empty<(int, int)>() };
    yield return new object[] { EdgeDirection.Forward, ForwardEdges };
    yield return new object[] { EdgeDirection.Backward, BackwardEdges };
    yield return new object[] { EdgeDirection.ForwardAndBackward, ForwardEdges.Concat(BackwardEdges).ToArray() };
  }
}