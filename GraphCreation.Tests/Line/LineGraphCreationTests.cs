using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class LineGraphCreationTests
{
  [Test]
  public void LineGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new LineGraphCreationOption<int, int>
    {
      Length = 4,
      CreateNodeData = data => data.Position,
      CreateEdgeData = _ => 0,
    };

    // ACT
    var graphs = new GraphBase<int, int>[]
    {
      GraphCreator.MakeLine(options),
      GraphCreator.MakeIndexedLine(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { 0, 1, 2, 3 }));
  }

  [Test]
  public void LineGraph_HasExpectedEdgeData()
  {
    // ARRANGE
    var options = new LineGraphCreationOption<int, (int, int)>
    {
      Length = 4,
      CreateNodeData = _ => 0,
      CreateEdgeData = data => (data.OriginPosition, data.DestinationPosition),
      EdgeDirection = EdgeDirection.ForwardAndBackward,
    };
    var expectedEdges = new[] { (0, 1), (1, 2), (2, 3), (3, 2), (2, 1), (1, 0) };

    // ACT
    var graphs = new GraphBase<int, (int, int)>[]
    {
      GraphCreator.MakeLine(options),
      GraphCreator.MakeIndexedLine(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [TestCaseSource(nameof(EdgeDirectionAndWrapAndExpectedEdgesForLengthThreeLineGraph))]
  public void LineGraph_HasExpectedStructure(EdgeDirection direction, bool wrap, (int from, int to)[] edges)
  {
    // ARRANGE
    var options = new GridGraphCreationOption<int, int>
    {
      DimensionInformation = new GridGraphDimensionInformation[]
      {
        new() { Length = 3, Wrap = wrap, EdgeDirection = direction },
      },
      CreateNodeData = data => data.Coordinates[0],
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

  [Test]
  public void IndexedLineGraph_HasExpectedIndices()
  {
    // ARRANGE
    var options = new LineGraphCreationOption<int, int>
    {
      Length = 5,
      CreateNodeData = data => data.Position,
      CreateEdgeData = _ => 0,
    };
    var expectedIndices = Enumerable.Range(0, 5);

    // ACT
    var indexedGraph = GraphCreator.MakeIndexedLine(options);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(expectedIndices));
  }

  private static IEnumerable<object[]> EdgeDirectionAndWrapAndExpectedEdgesForLengthThreeLineGraph()
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