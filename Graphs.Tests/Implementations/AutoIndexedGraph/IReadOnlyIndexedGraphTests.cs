using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IReadOnlyIndexedGraphTests : IReadOnlyIndexedGraphTests<AutoIndexedGraph<int, int, int>>
{
  protected override AutoIndexedGraph<int, int, int> EmptyGraph => new(n => n);

  protected override (AutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(0xC0FFEE);
      return (graph, node);
    }
  }

  protected override (AutoIndexedGraph<int, int, int> graph, int[] expectedIndices) GraphWithNodeIndices
  {
    get
    {
      var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
      var graph = new AutoIndexedGraph<int, int, int>(n => n);
      foreach (var index in indices)
        graph.AddNode(index);
      return (graph, indices);
    }
  }

  protected override (AutoIndexedGraph<int, int, int> graph, int removedIndex) EmptyGraphWithRemovedNodeIndex
  {
    get
    {
      const int index = 0xC0FFEE;
      var graph = new AutoIndexedGraph<int, int, int>(n => n);
      graph.AddNode(index);
      graph.RemoveNode(index);
      return (graph, index);
    }
  }
}