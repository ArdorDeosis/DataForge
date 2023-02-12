using System;
using System.Collections.Generic;
using System.Linq;
using DataForge.Graphs;
using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Tree;

internal static class TreeIndexHelper
{
  internal static TreeIndex FromArray(params int[] childIndices)
  {
    return childIndices.Aggregate(new TreeIndex(), (lastIndex, childIndex) => new TreeIndex(lastIndex, childIndex));
  }
}

internal class TreeGraphCreationTests
{
  [TestCaseSource(nameof(OptionsAndExpectedNodeIndicesOrData))]
  // this tests the node data creation the node child count calculation and the max depth parameter
  public void TreeGraph_HasExpectedNodeData(TreeGraphCreationOptions<TreeIndex, int> options,
    TreeIndex[] expectedNodeData)
  {
    // ACT
    var graphs = new IGraph<TreeIndex, int>[]
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
  public void TreeGraph_HasExpectedStructure(TreeGraphCreationOptions<TreeIndex, (TreeIndex, TreeIndex)> options,
    (TreeIndex, TreeIndex)[] expectedEdgeData)
  {
    // ACT
    var graphs = new IGraph<TreeIndex, (TreeIndex, TreeIndex)>[]
    {
      GraphCreator.MakeTree(options),
      GraphCreator.MakeIndexedTree(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => (edge.Origin.Data, edge.Destination.Data)),
        Is.EquivalentTo(expectedEdgeData));
  }

  [TestCaseSource(nameof(OptionsAndExpectedEdgeData))]
  // this tests the edge data creation and the edge direction parameters
  public void TreeGraph_HasExpectedEdgeData(TreeGraphCreationOptions<TreeIndex, (TreeIndex, TreeIndex)> options,
    (TreeIndex, TreeIndex)[] expectedEdgeData)
  {
    // ACT
    var graphs = new IGraph<TreeIndex, (TreeIndex, TreeIndex)>[]
    {
      GraphCreator.MakeTree(options),
      GraphCreator.MakeIndexedTree(options),
    };

    // ASSERT
    foreach (var graph in graphs)
      Assert.That(graph.Edges.Select(edge => edge.Data), Is.EquivalentTo(expectedEdgeData));
  }

  [TestCaseSource(nameof(OptionsAndExpectedNodeIndicesOrData))]
  public void IndexedTreeGraph_HasExpectedIndices(TreeGraphCreationOptions<TreeIndex, int> options,
    TreeIndex[] expectedIndices)
  {
    // ACT
    var graph = GraphCreator.MakeIndexedTree(options);

    // ASSERT
    Assert.That(graph.Indices, Is.EquivalentTo(expectedIndices));
  }

  private static IEnumerable<object[]> OptionsAndExpectedNodeIndicesOrData()
  {
    TreeIndex CreateNodeData(TreeIndex data) => data;

    int CreateEdgeData(IndexedGraphEdgeDataCreationInput<TreeIndex, TreeIndex> _) => 0;

    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, int>
      {
        MaxDepth = 0,
        CalculateChildNodeCount = (_, _) => 0xBEEF,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new[] { new TreeIndex() },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, int>
      {
        MaxDepth = 2,
        CalculateChildNodeCount = (_, _) => 2,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new[]
      {
        new(),
        TreeIndexHelper.FromArray(0),
        TreeIndexHelper.FromArray(1),
        TreeIndexHelper.FromArray(0, 0),
        TreeIndexHelper.FromArray(0, 1),
        TreeIndexHelper.FromArray(1, 0),
        TreeIndexHelper.FromArray(1, 1),
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, int>
      {
        CalculateChildNodeCount = (_, address) => 3 - address.Depth,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
      },
      new[]
      {
        new(),
        TreeIndexHelper.FromArray(0),
        TreeIndexHelper.FromArray(1),
        TreeIndexHelper.FromArray(2),
        TreeIndexHelper.FromArray(0, 0),
        TreeIndexHelper.FromArray(0, 1),
        TreeIndexHelper.FromArray(1, 0),
        TreeIndexHelper.FromArray(1, 1),
        TreeIndexHelper.FromArray(2, 0),
        TreeIndexHelper.FromArray(2, 1),
        TreeIndexHelper.FromArray(0, 0, 0),
        TreeIndexHelper.FromArray(0, 1, 0),
        TreeIndexHelper.FromArray(1, 0, 0),
        TreeIndexHelper.FromArray(1, 1, 0),
        TreeIndexHelper.FromArray(2, 0, 0),
        TreeIndexHelper.FromArray(2, 1, 0),
      },
    };
  }

  private static IEnumerable<object[]> OptionsAndExpectedEdgeData()
  {
    const int maxDepth = 2;

    TreeIndex CreateNodeData(TreeIndex index) => index;

    (TreeIndex from, TreeIndex to) CreateEdgeData(IndexedGraphEdgeDataCreationInput<TreeIndex, TreeIndex> data) =>
      (data.StartIndex, data.EndIndex);

    int CalculateChildNodeCount(TreeIndex addressData, TreeIndex _) => addressData.ChildIndex == 0 ? 2 : 0;

    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, (TreeIndex from, TreeIndex to)>
      {
        MaxDepth = maxDepth,
        CalculateChildNodeCount = CalculateChildNodeCount,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.None,
      },
      Array.Empty<(TreeIndex, TreeIndex)>(),
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, (TreeIndex from, TreeIndex to)>
      {
        MaxDepth = maxDepth,
        CalculateChildNodeCount = CalculateChildNodeCount,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.Forward,
      },
      new[]
      {
        (new TreeIndex(), TreeIndexHelper.FromArray(0)),
        (new TreeIndex(), TreeIndexHelper.FromArray(1)),
        (TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0, 0)),
        (TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0, 1)),
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, (TreeIndex from, TreeIndex to)>
      {
        MaxDepth = maxDepth,
        CalculateChildNodeCount = CalculateChildNodeCount,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.Backward,
      },
      new[]
      {
        (TreeIndexHelper.FromArray(0), new TreeIndex()),
        (TreeIndexHelper.FromArray(1), new TreeIndex()),
        (TreeIndexHelper.FromArray(0, 0), TreeIndexHelper.FromArray(0)),
        (TreeIndexHelper.FromArray(0, 1), TreeIndexHelper.FromArray(0)),
      },
    };
    yield return new object[]
    {
      new TreeGraphCreationOptions<TreeIndex, (TreeIndex from, TreeIndex to)>
      {
        MaxDepth = maxDepth,
        CalculateChildNodeCount = CalculateChildNodeCount,
        CreateNodeData = CreateNodeData,
        CreateEdgeData = CreateEdgeData,
        EdgeDirection = EdgeDirection.ForwardAndBackward,
      },
      new[]
      {
        (new TreeIndex(), TreeIndexHelper.FromArray(0)),
        (new TreeIndex(), TreeIndexHelper.FromArray(1)),
        (TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0, 0)),
        (TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0, 1)),
        (TreeIndexHelper.FromArray(0), new TreeIndex()),
        (TreeIndexHelper.FromArray(1), new TreeIndex()),
        (TreeIndexHelper.FromArray(0, 0), TreeIndexHelper.FromArray(0)),
        (TreeIndexHelper.FromArray(0, 1), TreeIndexHelper.FromArray(0)),
      },
    };
  }
}