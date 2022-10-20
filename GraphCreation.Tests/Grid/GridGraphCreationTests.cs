using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class GridGraphCreationTests
{
  [Test]
  public void GridGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new GridGraphCreationOptions<int, int>
    {
      DimensionInformation = new GridGraphDimensionInformation[]
      {
        new() { Length = 2 },
        new() { Length = 2 },
      },
      CreateNodeData = data => data[0] * 2 + data[1],
      CreateEdgeData = _ => 0,
    };

    // ACT
    var graphs = new GraphBase<int, int>[]
    {
      GraphCreator.MakeGrid(options),
      GraphCreator.MakeIndexedGrid(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { 0, 1, 2, 3 }));
  }

  [Test]
  public void GridGraph_HasExpectedEdgeData()
  {
    // ARRANGE
    var edgeData = new[] { "Horst", "Hermann", "Kurt", "Willy" };
    var edgeCounter = 0;
    var options = new GridGraphCreationOptions<int, string>
    {
      DimensionInformation = new GridGraphDimensionInformation[]
      {
        new() { Length = 2 },
        new() { Length = 2 },
      },
      CreateNodeData = _ => 0,
      CreateEdgeData = _ => edgeData[edgeCounter++ % 4],
    };

    // ACT
    var graphs = new GraphBase<int, string>[]
    {
      GraphCreator.MakeGrid(options),
      GraphCreator.MakeIndexedGrid(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(edgeData));
  }

  [TestCaseSource(nameof(EdgeDirectionAndWrapAndExpectedEdgesForThreeCoordinatesInOneDimension))]
  public void GridGraph_HasExpectedStructure(EdgeDirection direction, bool wrap, (int from, int to)[] edges)
  {
    // ARRANGE
    var options = new GridGraphCreationOptions<int, int>
    {
      DimensionInformation = new GridGraphDimensionInformation[]
      {
        new() { Length = 3, Wrap = wrap, EdgeDirection = direction },
      },
      CreateNodeData = coordinate => coordinate[0],
      CreateEdgeData = _ => 0,
    };

    // ACT
    var graphs = new GraphBase<int, int>[]
    {
      GraphCreator.MakeGrid(options),
      GraphCreator.MakeIndexedGrid(options),
    };

    // ASSERT
    foreach (var graph in graphs)
    {
      Assert.That(graph.Edges, Has.Count.EqualTo(edges.Length));
      Assert.That(graph.Edges.Select(edge => (edge.Start.Data, edge.End.Data)), Is.EquivalentTo(edges));
    }
  }

  [TestCaseSource(nameof(GridSizeAndExpectedCoordinates))]
  public void IndexedGridGraph_HasExpectedIndices(IReadOnlyList<int> gridSize, int[][] expectedCoordinates)
  {
    // ARRANGE
    var options = new GridGraphCreationOptions<IReadOnlyList<int>, int>
    {
      DimensionInformation = gridSize.Select(size => new GridGraphDimensionInformation { Length = size }).ToArray(),
      CreateNodeData = coordinate => coordinate,
      CreateEdgeData = _ => 0,
    };

    // ACT
    var indexedGraph = GraphCreator.MakeIndexedGrid(options);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(expectedCoordinates));
  }

  private static IEnumerable<object[]> GridSizeAndExpectedCoordinates()
  {
    yield return new object[]
    {
      new[] { 2 },
      new[] { new[] { 0 }, new[] { 1 } },
    };
    yield return new object[]
    {
      new[] { 2, 2 },
      new[] { new[] { 0, 0 }, new[] { 0, 1 }, new[] { 1, 0 }, new[] { 1, 1 } },
    };
    yield return new object[]
    {
      new[] { 1, 1, 1 },
      new[] { new[] { 0, 0, 0 } },
    };
  }

  private static IEnumerable<object[]> EdgeDirectionAndWrapAndExpectedEdgesForThreeCoordinatesInOneDimension()
  {
    yield return new object[] { EdgeDirection.None, false, Array.Empty<(int, int)>() };
    yield return new object[] { EdgeDirection.None, true, Array.Empty<(int, int)>() };
    yield return new object[] { EdgeDirection.Forward, false, new[] { (0, 1), (1, 2) } };
    yield return new object[] { EdgeDirection.Forward, true, new[] { (0, 1), (1, 2), (2, 0) } };
    yield return new object[] { EdgeDirection.Backward, false, new[] { (2, 1), (1, 0) } };
    yield return new object[] { EdgeDirection.Backward, true, new[] { (2, 1), (1, 0), (0, 2) } };
    yield return new object[] { EdgeDirection.ForwardAndBackward, false, new[] { (0, 1), (1, 2), (2, 1), (1, 0) } };
    yield return new object[]
      { EdgeDirection.ForwardAndBackward, true, new[] { (0, 1), (1, 2), (2, 0), (2, 1), (1, 0), (0, 2) } };
  }
}