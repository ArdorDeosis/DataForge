using System.Linq;
using Graph;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class DiskGraphCreationTests
{
  [Test]
  public void DiskGraph_HasExpectedNodeData()
  {
    // ARRANGE
    var options = new DiskGraphCreationOption<(int ray, int distance), int>
    {
      CreateNodeData = index => (index.Meridian, index.Distance),
      CreateEdgeData = _ => 0,
      MeridianCount = 3,
      RingCount = 3,
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
      GraphCreator.MakeDisk(options),
      GraphCreator.MakeIndexedDisk(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(expectedNodeData));
  }

  [Test]
  public void DiskGraph_HasExpectedEdgeData()
  {
    // ARRANGE
    var options = new DiskGraphCreationOption<DiskIndex, (DiskIndex, DiskIndex)>
    {
      MeridianCount = 3,
      RingCount = 2,
      CreateNodeData = index => index,
      CreateEdgeData = data => (
        data.OriginIndex,
        data.DestinationIndex
      ),
      MeridianEdgeDirection = EdgeDirection.Backward,
      RingEdgeDirection = EdgeDirection.ForwardAndBackward,
    };
    var expectedEdges = new[]
    {
      (new DiskIndex(0, 1), new DiskIndex()),
      (new DiskIndex(1, 1), new DiskIndex()),
      (new DiskIndex(2, 1), new DiskIndex()),
      (new DiskIndex(0, 2), new DiskIndex(0, 1)),
      (new DiskIndex(1, 2), new DiskIndex(1, 1)),
      (new DiskIndex(2, 2), new DiskIndex(2, 1)),

      (new DiskIndex(0, 1), new DiskIndex(1, 1)),
      (new DiskIndex(1, 1), new DiskIndex(0, 1)),
      (new DiskIndex(1, 1), new DiskIndex(2, 1)),
      (new DiskIndex(2, 1), new DiskIndex(1, 1)),
      (new DiskIndex(2, 1), new DiskIndex(0, 1)),
      (new DiskIndex(0, 1), new DiskIndex(2, 1)),
      (new DiskIndex(0, 2), new DiskIndex(1, 2)),
      (new DiskIndex(1, 2), new DiskIndex(0, 2)),
      (new DiskIndex(1, 2), new DiskIndex(2, 2)),
      (new DiskIndex(2, 2), new DiskIndex(1, 2)),
      (new DiskIndex(2, 2), new DiskIndex(0, 2)),
      (new DiskIndex(0, 2), new DiskIndex(2, 2)),
    };

    // ACT
    var graphs = new GraphBase<DiskIndex, (DiskIndex, DiskIndex)>[]
    {
      GraphCreator.MakeDisk(options),
      GraphCreator.MakeIndexedDisk(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdges));
  }

  [Test]
  public void DiskGraph_HasExpectedStructure()
  {
    // ARRANGE
    var options = new DiskGraphCreationOption<DiskIndex, int>
    {
      MeridianCount = 3,
      RingCount = 2,
      CreateNodeData = index => index,
      CreateEdgeData = _ => 0,
      MeridianEdgeDirection = EdgeDirection.Forward,
      RingEdgeDirection = EdgeDirection.Forward,
    };
    var expectedEdges = new[]
    {
      (new DiskIndex(), new DiskIndex(0, 1)),
      (new DiskIndex(), new DiskIndex(1, 1)),
      (new DiskIndex(), new DiskIndex(2, 1)),
      (new DiskIndex(0, 1), new DiskIndex(0, 2)),
      (new DiskIndex(1, 1), new DiskIndex(1, 2)),
      (new DiskIndex(2, 1), new DiskIndex(2, 2)),
      (new DiskIndex(0, 1), new DiskIndex(1, 1)),
      (new DiskIndex(1, 1), new DiskIndex(2, 1)),
      (new DiskIndex(2, 1), new DiskIndex(0, 1)),
      (new DiskIndex(0, 2), new DiskIndex(1, 2)),
      (new DiskIndex(1, 2), new DiskIndex(2, 2)),
      (new DiskIndex(2, 2), new DiskIndex(0, 2)),
    };

    // ACT
    var graphs = new GraphBase<DiskIndex, int>[]
    {
      GraphCreator.MakeDisk(options),
      GraphCreator.MakeIndexedDisk(options),
    };

    // ASSERT
    foreach (var graph in graphs)
    {
      Assert.That(graph.Edges, Has.Count.EqualTo(expectedEdges.Length));
      Assert.That(graph.Edges.Select(edge => (edge.Start.Data, edge.End.Data)), Is.EquivalentTo(expectedEdges));
    }
  }

  [Test]
  public void IndexedDiskGraph_HasExpectedIndices()
  {
    // ARRANGE
    var options = new DiskGraphCreationOption<int, int>
    {
      CreateNodeData = _ => 0,
      CreateEdgeData = _ => 0,
      MeridianCount = 3,
      RingCount = 2,
      MakeCenterNode = false,
    };
    var expectedIndices = new DiskIndex[]
    {
      new(0, 1), new(0, 2),
      new(1, 1), new(1, 2),
      new(2, 1), new(2, 2),
    };

    // ACT
    var indexedGraph = GraphCreator.MakeIndexedDisk(options);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(expectedIndices));
  }
}