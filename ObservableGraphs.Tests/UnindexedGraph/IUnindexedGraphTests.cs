using DataForge.Graphs;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.UnindexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IUnindexedGraphTests : IUnindexedGraphTests<ObservableGraph<int, int>>
{
  protected override ObservableGraph<int, int> EmptyGraph => new();

  protected override (ObservableGraph<int, int> graph, Node<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new ObservableGraph<int, int>();
      var node = graph.AddNode(default);
      graph.RemoveNode(node);
      return (graph, node);
    }
  }

  protected override Node<int, int> NodeFromOtherGraph => new ObservableGraph<int, int>().AddNode(default);
}