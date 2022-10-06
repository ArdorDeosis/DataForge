using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using NUnit.Framework;

namespace GraphCreation.Tests;

public class TreeGraphCreationTests
{
  [TestCaseSource(nameof(OptionsAndExpectedNodeIndicesOrData))]
  // this tests the node data creation the node child count calculation and the max depth parameter
  public void TreeGraph_HasExpectedNodeData(TreeGraphCreationOptions<IReadOnlyList<int>, int> options,
    IReadOnlyList<int>[] expectedNodeData)
  {
    // ACT
    var graphs = new GraphBase<IReadOnlyList<int>, int>[]
    {
      GraphCreator.MakeTree(options),
      GraphCreator.MakeIndexedTree(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Nodes.Select(node => node.Data), Is.EquivalentTo(expectedNodeData));
  }

  [TestCaseSource(nameof(OptionsAndExpectedEdgeData))]
  // this tests the edge data creation and the edge direction parameters
  public void TreeGraph_HasExpectedEdgeData(TreeGraphCreationOptions<int, (int[], int[])> options,
    (int[], int[])[] expectedEdgeData)
  {
    // ACT
    var graphs = new GraphBase<int, (int[], int[])>[]
    {
      GraphCreator.MakeTree(options),
      GraphCreator.MakeIndexedTree(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdgeData));
  }

  [TestCaseSource(nameof(OptionsAndExpectedNodeIndicesOrData))]
  public void IndexedTreeGraph_HasExpectedIndices(TreeGraphCreationOptions<IReadOnlyList<int>, int> options,
    IReadOnlyList<int>[] expectedIndices)
  {
    // ACT
    var graph = GraphCreator.MakeIndexedTree(options);

    // ASSERT
    Assert.That(graph.Indices, Is.EquivalentTo(expectedIndices));
  }

  private static IEnumerable<object[]> OptionsAndExpectedNodeIndicesOrData()
  {
    IReadOnlyList<int> CreateNodeData(TreeNodeData data) => data.Address;
    int CreateEdgeData(TreeEdgeData<IReadOnlyList<int>> _) => 0;

    yield return new object[]
    {
      new TreeGraphCreationOptions<IReadOnlyList<int>, int>
      {
        MaxDepth = 0,
        CalculateChildNodeCount = (_, _) => 0xBEEF,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new[] { Array.Empty<int>() },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<IReadOnlyList<int>, int>
      {
        MaxDepth = 2,
        CalculateChildNodeCount = (_, _) => 2,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new IReadOnlyList<int>[]
      {
        Array.Empty<int>(),
        new[] { 0 },
        new[] { 1 },
        new[] { 0, 0 },
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { 1, 1 },
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<IReadOnlyList<int>, int>
      {
        MaxDepth = 10, // TODO: should be null
        CalculateChildNodeCount = (_, address) => 3 - address.Count,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new IReadOnlyList<int>[]
      {
        Array.Empty<int>(),
        new[] { 0 },
        new[] { 1 },
        new[] { 2 },
        new[] { 0, 0 },
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { 1, 1 },
        new[] { 2, 0 },
        new[] { 2, 1 },
        new[] { 0, 0, 0 },
        new[] { 0, 1, 0 },
        new[] { 1, 0, 0 },
        new[] { 1, 1, 0 },
        new[] { 2, 0, 0 },
        new[] { 2, 1, 0 },
      },
    };
  }

  private static IEnumerable<object[]> OptionsAndExpectedEdgeData()
  {
    int CreateNodeData(TreeNodeData _) => 0;

    (int[] from, int[] to) CreateEdgeData(TreeEdgeData<int> data) =>
      (data.OriginAddress.ToArray(), data.DestinationAddress.ToArray());

    yield return new object[]
    {
      new TreeGraphCreationOptions<int, (int[] from, int[] to)>
      {
        MaxDepth = 1,
        CalculateChildNodeCount = (addressData, _) =>
          addressData.Address.Count == 0 || addressData.Address[^1] == 0 ? 2 : 0,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.None,
      },
      Array.Empty<(int[], int[])>(),
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<int, (int[] from, int[] to)>
      {
        MaxDepth = 2,
        CalculateChildNodeCount = (addressData, _) =>
          addressData.Address.Count == 0 || addressData.Address[^1] == 0 ? 2 : 0,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.Forward,
      },
      new[]
      {
        (Array.Empty<int>(), new[] { 0 }),
        (Array.Empty<int>(), new[] { 1 }),
        (new[] { 0 }, new[] { 0, 0 }),
        (new[] { 0 }, new[] { 0, 1 }),
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<int, (int[] from, int[] to)>
      {
        MaxDepth = 2,
        CalculateChildNodeCount = (addressData, _) =>
          addressData.Address.Count == 0 || addressData.Address[^1] == 0 ? 2 : 0,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.Backward,
      },
      new[]
      {
        (new[] { 0 }, Array.Empty<int>()),
        (new[] { 1 }, Array.Empty<int>()),
        (new[] { 0, 0 }, new[] { 0 }),
        (new[] { 0, 1 }, new[] { 0 }),
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<int, (int[] from, int[] to)>
      {
        MaxDepth = 2,
        CalculateChildNodeCount = (addressData, _) =>
          addressData.Address.Count == 0 || addressData.Address[^1] == 0 ? 2 : 0,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.ForwardAndBackward,
      },
      new[]
      {
        (Array.Empty<int>(), new[] { 0 }),
        (Array.Empty<int>(), new[] { 1 }),
        (new[] { 0 }, new[] { 0, 0 }),
        (new[] { 0 }, new[] { 0, 1 }),
        (new[] { 0 }, Array.Empty<int>()),
        (new[] { 1 }, Array.Empty<int>()),
        (new[] { 0, 0 }, new[] { 0 }),
        (new[] { 0, 1 }, new[] { 0 }),
      },
    };
  }
}