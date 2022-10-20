using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class StarGraphCreationTests
{
  [Test]
  public void StarGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new StarGraphCreationOptions<(int ray, int distance), int>
    {
      CreateNodeData = index => (index.Ray, index.Distance),
      CreateEdgeData = _ => 0,
      RayCount = 3,
      EdgeDirection = EdgeDirection.None,
      CalculateRayLength = _ => 3,
    };
    var expectedNodeData = new[]
    {
      (0, 0),
      (0, 1), (0, 2), (0, 3),
      (1, 1), (1, 2), (1, 3),
      (2, 1), (2, 2), (2, 3),
    };

    // ACT
    var graphs = new GraphBase<(int, int), int>[]
    {
      GraphCreator.MakeStar(options),
      GraphCreator.MakeIndexedStar(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(expectedNodeData));
  }

  [Test]
  public void StarGraph_HasExpectedEdgeData()
  {
    // ARRANGE
    var options = new StarGraphCreationOptions<(int ray, int distance), (int ray, int from, int to)>
    {
      RayCount = 3,
      CalculateRayLength = _ => 2,
      CreateNodeData = index => (index.Ray, index.Distance),
      CreateEdgeData = data => (
        Math.Max(data.StartIndex.Ray, data.EndIndex.Ray),
        data.StartIndex.Distance,
        data.EndIndex.Distance
      ),
      EdgeDirection = EdgeDirection.ForwardAndBackward,
    };
    var expectedEdges = new[]
    {
      (0, 0, 1), (0, 1, 2), (0, 1, 0), (0, 2, 1),
      (1, 0, 1), (1, 1, 2), (1, 1, 0), (1, 2, 1),
      (2, 0, 1), (2, 1, 2), (2, 1, 0), (2, 2, 1),
    };

    // ACT
    var graphs = new GraphBase<(int ray, int distance), (int ray, int from, int to)>[]
    {
      GraphCreator.MakeStar(options),
      GraphCreator.MakeIndexedStar(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [TestCaseSource(nameof(StructureTestData))]
  public void StarGraph_HasExpectedStructure(EdgeDirection direction, (int ray, int from, int to)[] expectedEdges)
  {
    // ARRANGE
    var options = new StarGraphCreationOptions<(int ray, int distance), int>
    {
      RayCount = 3,
      CalculateRayLength = _ => 2,
      CreateNodeData = index => (index.Ray, index.Distance),
      CreateEdgeData = _ => 0,
      EdgeDirection = direction,
    };

    // ACT
    var graphs = new GraphBase<(int ray, int distance), int>[]
    {
      GraphCreator.MakeStar(options),
      GraphCreator.MakeIndexedStar(options),
    };

    // ASSERT
    foreach (var graph in graphs)
    {
      Assert.That(graph.Edges, Has.Count.EqualTo(expectedEdges.Length));
      var edges = graph.Edges.Select(edge => (
        Math.Max(edge.Start.Data.ray, edge.End.Data.ray),
        edge.Start.Data.distance,
        edge.End.Data.distance
      ));
      Assert.That(edges, Is.EquivalentTo(expectedEdges));
    }
  }

  [Test]
  // this also asserts the ray length calculation
  public void IndexedStarGraph_HasExpectedIndices()
  {
    // ARRANGE
    var options = new StarGraphCreationOptions<int, int>
    {
      RayCount = 3,
      CalculateRayLength = n => 3 - n,
      CreateNodeData = _ => 0,
      CreateEdgeData = _ => 0,
    };
    var expectedIndices = new StarIndex[]
    {
      new(),
      new(0, 1), new(0, 2), new(0, 3),
      new(1, 1), new(1, 2),
      new(2, 1),
    };

    // ACT
    var indexedGraph = GraphCreator.MakeIndexedStar(options);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(expectedIndices));
  }

  private static IEnumerable<object[]> StructureTestData()
  {
    yield return new object[] { EdgeDirection.None, Array.Empty<(int, int, int)>() };
    yield return new object[]
    {
      EdgeDirection.Forward, new[]
      {
        (0, 0, 1), (0, 1, 2),
        (1, 0, 1), (1, 1, 2),
        (2, 0, 1), (2, 1, 2),
      },
    };
    yield return new object[]
    {
      EdgeDirection.Backward, new[]
      {
        (0, 2, 1), (0, 1, 0),
        (1, 2, 1), (1, 1, 0),
        (2, 2, 1), (2, 1, 0),
      },
    };
    yield return new object[]
    {
      EdgeDirection.ForwardAndBackward, new[]
      {
        (0, 0, 1), (0, 1, 2), (0, 2, 1), (0, 1, 0),
        (1, 0, 1), (1, 1, 2), (1, 2, 1), (1, 1, 0),
        (2, 0, 1), (2, 1, 2), (2, 2, 1), (2, 1, 0),
      },
    };
  }
}