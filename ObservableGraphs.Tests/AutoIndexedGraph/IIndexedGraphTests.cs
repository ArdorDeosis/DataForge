using DataForge.Graphs;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IIndexedGraphTests : IIndexedGraphTests<ObservableAutoIndexedGraph<int, int, int>>
{
  protected override ObservableAutoIndexedGraph<int, int, int> EmptyGraph =>
    new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> expectedNode)
    GraphWithNode
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> node1,
    IndexedNode<int, int, int> node2) GraphWithTwoNodes
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      return (graph, graph.AddNode(default), graph.AddNode(default));
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedEdge<int, int, int> expectedEdge)
    GraphWithEdge
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default);
      return (graph, edge);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> removedNode)
    EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      graph.RemoveNode(node);
      return (graph, node);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IndexedEdge<int, int, int> removedEdge)
    EmptyGraphWithRemovedEdge
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    }
  }

  protected override IndexedNode<int, int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IndexedEdge<int, int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}