using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IReadOnlyIndexedGraphTests : IReadOnlyIndexedGraphTests<IndexedGraph<int, int, int>>
{
  protected override IndexedGraph<int, int, int> EmptyGraph => new();

  protected override (IndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      var node = graph.AddNode(0xC0FFEE, default);
      return (graph, node);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, int[] expectedIndices) GraphWithNodeIndices
  {
    get
    {
      var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
      var graph = new IndexedGraph<int, int, int>();
      foreach (var index in indices)
        graph.AddNode(index, default);
      return (graph, indices);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, int removedIndex) EmptyGraphWithRemovedNodeIndex
  {
    get
    {
      const int index = 0xC0FFEE;
      var graph = new IndexedGraph<int, int, int>();
      graph.AddNode(index, default);
      graph.RemoveNode(index);
      return (graph, index);
    }
  }
}