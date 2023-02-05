using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IAutoIndexedGraphTests : IAutoIndexedGraphTests<AutoIndexedGraph<int, int, int>>
{
  protected override AutoIndexedGraph<int, int, int> EmptyGraphTakingIndicesFromData => new(data => data);

  protected override (AutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode)
    GraphWithNodeTakingIndicesFromData
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(data => data);
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }
}