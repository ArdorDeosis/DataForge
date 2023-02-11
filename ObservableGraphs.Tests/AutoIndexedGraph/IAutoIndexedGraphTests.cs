using DataForge.Graphs;
using DataForge.Graphs.Observable;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IObservableAutoIndexedGraphTests : IAutoIndexedGraphTests<ObservableAutoIndexedGraph<int, int, int>>
{
  protected override ObservableAutoIndexedGraph<int, int, int> EmptyGraphTakingIndicesFromData => new(data => data);

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode)
    GraphWithNodeTakingIndicesFromData
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(data => data);
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }
}