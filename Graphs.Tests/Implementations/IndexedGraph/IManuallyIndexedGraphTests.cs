using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IManuallyIndexedGraphTests : IManuallyIndexedGraphTests<IndexedGraph<int, int, int>>
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
}