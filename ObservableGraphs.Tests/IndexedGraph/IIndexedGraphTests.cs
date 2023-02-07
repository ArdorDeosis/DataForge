using DataForge.Graphs;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IObservableIndexedGraphTests : IIndexedGraphTests<ObservableIndexedGraph<int, int, int>>
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

  protected override (ObservableIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> node1,
    IndexedNode<int, int, int>
    node2) GraphWithTwoNodes
  {
    get
    {
      var graph = new ObservableIndexedGraph<int, int, int>();
      return (graph, graph.AddNode(0xC0FFEE, default), graph.AddNode(0xBEEF, default));
    }
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IndexedEdge<int, int, int> expectedEdge)
    GraphWithEdge
  {
    get
    {
      const int index1 = 0xC0FFEE;
      const int index2 = 0xF00D;
      var graph = new ObservableIndexedGraph<int, int, int>();
      graph.AddNode(index1, default);
      graph.AddNode(index2, default);
      var edge = graph.AddEdge(index1, index2, default);
      return (graph, edge);
    }
  }


  protected override (ObservableIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> removedNode)
    EmptyGraphWithRemovedNode
  {
    get
    {
      const int index = 0xC0FFEE;
      var graph = new ObservableIndexedGraph<int, int, int>();
      var node = graph.AddNode(index, default);
      graph.RemoveNode(index);
      return (graph, node);
    }
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IndexedEdge<int, int, int> removedEdge)
    EmptyGraphWithRemovedEdge
  {
    get
    {
      const int index1 = 0xC0FFEE;
      const int index2 = 0xF00D;
      var graph = new ObservableIndexedGraph<int, int, int>();
      graph.AddNode(index1, default);
      graph.AddNode(index2, default);
      var edge = graph.AddEdge(index1, index2, default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    }
  }

  protected override IndexedNode<int, int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IndexedEdge<int, int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}