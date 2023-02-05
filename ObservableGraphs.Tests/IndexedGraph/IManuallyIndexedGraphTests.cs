using DataForge.Graphs;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IManuallyObservableIndexedGraphTests : IManuallyIndexedGraphTests<ObservableIndexedGraph<int, int, int>>
{
  protected override ObservableIndexedGraph<int, int, int> EmptyGraph => new();

  protected override (ObservableIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode)
    GraphWithNode
  {
    get
    {
      var graph = new ObservableIndexedGraph<int, int, int>();
      var node = graph.AddNode(0xC0FFEE, default);
      return (graph, node);
    }
  }
}