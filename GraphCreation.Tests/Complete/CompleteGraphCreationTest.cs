using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Complete;

public class CompleteGraphCreationTest
{
  [TestCaseSource(nameof(NodeDataTestSets))]
  public void CompleteGraph_AllNodesAreCreated(int[] nodeData)
  {
    // ARRANGE
    var options = new CompleteGraphCreationOptions<int, int>
    {
      NodeData = nodeData,
      CreateEdgeData = (_, _) => 0,
      EdgeDirection = EdgeDirection.None,
    };

    // ACT
    var graph = GraphCreator.MakeComplete(options);

    // ASSERT
    Assert.That(graph.Nodes.Select(node => node.Data), Is.EqualTo(nodeData));
  }

  [TestCaseSource(nameof(EdgeDirectionAndEdgesForFiveNodes))]
  public void CompleteGraph_HasCorrectStructure(EdgeDirection direction, (int, int)[] expectedEdges)
  {
    // ARRANGE
    var data = Enumerable.Range(0, 5);
    var options = new CompleteGraphCreationOptions<int, int>
    {
      NodeData = data,
      CreateEdgeData = (_, _) => 0,
      EdgeDirection = direction,
    };

    // ACT
    var graph = GraphCreator.MakeComplete(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)), Is.EquivalentTo(expectedEdges));
  }

  [Test]
  public void CompleteGraph_EdgeDataIsCreatedCorrectly()
  {
    // ARRANGE
    var data = Enumerable.Range(0, 3);
    var options = new CompleteGraphCreationOptions<int, int>
    {
      NodeData = data,
      CreateEdgeData = (from, to) => from + to,
      EdgeDirection = EdgeDirection.Forward,
    };
    var expectedEdgeData = new[] { 1, 2, 3 };

    // ACT
    var graph = GraphCreator.MakeComplete(options);

    // ASSERT
    Assert.That(graph.Edges.Select(edge => edge.Data), Is.EqualTo(expectedEdgeData));
  }

  private static IEnumerable<int[]> NodeDataTestSets()
  {
    yield return Array.Empty<int>();
    yield return new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    yield return new[] { 0, 0 };
    yield return Enumerable.Range(0, 1000).ToArray();
  }

  private static IEnumerable<object[]> EdgeDirectionAndEdgesForFiveNodes()
  {
    yield return new object[] { EdgeDirection.None, Array.Empty<(int, int)>() };
    yield return new object[]
    {
      EdgeDirection.Forward,
      new[]
      {
        (0, 1), (0, 2), (0, 3), (0, 4),
        (1, 2), (1, 3), (1, 4),
        (2, 3), (2, 4),
        (3, 4),
      },
    };
    yield return new object[]
    {
      EdgeDirection.Backward,
      new[]
      {
        (1, 0),
        (2, 0), (2, 1),
        (3, 0), (3, 1), (3, 2),
        (4, 0), (4, 1), (4, 2), (4, 3),
      },
    };
    yield return new object[]
    {
      EdgeDirection.ForwardAndBackward,
      new[]
      {
        (0, 1), (0, 2), (0, 3), (0, 4),
        (1, 0), (1, 2), (1, 3), (1, 4),
        (2, 0), (2, 1), (2, 3), (2, 4),
        (3, 0), (3, 1), (3, 2), (3, 4),
        (4, 0), (4, 1), (4, 2), (4, 3),
      },
    };
  }
}