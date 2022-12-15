using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class RingGraphCreationTests
{
  [Test]
  public void RingGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new RingGraphCreationOptions<int, int>
    {
      Size = 4,
      CreateNodeData = position => position,
      CreateEdgeData = _ => 0,
    };

    // ACT
    var graphs = new GraphBase<int, int>[]
    {
      GraphCreator.MakeRing(options),
      GraphCreator.MakeIndexedRing(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { 0, 1, 2, 3 }));
  }

  [Test]
  public void RingGraph_HasExpectedEdgeData()
  {
    // ARRANGE
    var options = new RingGraphCreationOptions<int, (int, int)>
    {
      Size = 4,
      CreateNodeData = _ => 0,
      CreateEdgeData = data => (OriginIndex: data.StartIndex, DestinationIndex: data.EndIndex),
      EdgeDirection = EdgeDirection.ForwardAndBackward,
    };
    var expectedEdges = new[] { (0, 1), (1, 2), (2, 3), (3, 0), (0, 3), (3, 2), (2, 1), (1, 0) };

    // ACT
    var graphs = new GraphBase<int, (int, int)>[]
    {
      GraphCreator.MakeRing(options),
      GraphCreator.MakeIndexedRing(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [TestCaseSource(nameof(EdgeDirectionAndExpectedEdgesForSizeThreeRingGraph))]
  public void RingGraph_HasExpectedStructure(EdgeDirection direction, (int from, int to)[] edges)
  {
    // ARRANGE
    var options = new RingGraphCreationOptions<int, int>
    {
      Size = 3,
      CreateNodeData = position => position,
      CreateEdgeData = _ => 0,
      EdgeDirection = direction,
    };

    // ACT
    var graphs = new GraphBase<int, int>[]
    {
      GraphCreator.MakeRing(options),
      GraphCreator.MakeIndexedRing(options),
    };

    // ASSERT
    foreach (var graph in graphs)
    {
      Assert.That(graph.Edges, Has.Count.EqualTo(edges.Length));
      Assert.That(graph.Edges.Select(edge => (edge.Start.Data, edge.End.Data)), Is.EquivalentTo(edges));
    }
  }

  [Test]
  public void IndexedRingGraph_HasExpectedIndices()
  {
    // ARRANGE
    var options = new RingGraphCreationOptions<int, int>
    {
      Size = 5,
      CreateNodeData = position => position,
      CreateEdgeData = _ => 0,
    };
    var expectedIndices = Enumerable.Range(0, 5);

    // ACT
    var indexedGraph = GraphCreator.MakeIndexedRing(options);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(expectedIndices));
  }

  private static IEnumerable<object[]> EdgeDirectionAndExpectedEdgesForSizeThreeRingGraph()
  {
    yield return new object[] { EdgeDirection.None, Array.Empty<(int, int)>() };
    yield return new object[] { EdgeDirection.Forward, new[] { (0, 1), (1, 2), (2, 0) } };
    yield return new object[] { EdgeDirection.Backward, new[] { (2, 1), (1, 0), (0, 2) } };
    yield return new object[]
      { EdgeDirection.ForwardAndBackward, new[] { (0, 1), (1, 2), (2, 0), (2, 1), (1, 0), (0, 2) } };
  }
}