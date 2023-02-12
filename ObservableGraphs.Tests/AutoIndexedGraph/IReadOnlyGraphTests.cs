using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Observable.Tests.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IReadOnlyGraphTests : IReadOnlyGraphTests<ObservableAutoIndexedGraph<int, int, int>>
{
  protected override ObservableAutoIndexedGraph<int, int, int> GetEmptyGraph =>
    new(new IncrementalIndexProvider<int, int>(0));

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, INode<int, int>[] expectedNodes) GraphWithNodes
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var nodes = new INode<int, int>[]
      {
        graph.AddNode(default),
        graph.AddNode(default),
        graph.AddNode(default),
      };
      return (graph, nodes);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IEdge<int, int>[] expectedEdges) GraphWithEdges
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node1 = graph.AddNode(default);
      var node2 = graph.AddNode(default);
      var edges = new IEdge<int, int>[]
      {
        graph.AddEdge(node1.Index, node1.Index, default),
        graph.AddEdge(node1.Index, node2.Index, default),
        graph.AddEdge(node2.Index, node1.Index, default),
      };
      return (graph, edges);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, INode<int, int> removedNode)
    EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(n => n);
      const int index = 0;
      var node = graph.AddNode(index);
      graph.RemoveNode(index);
      return (graph, node);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge)
    EmptyGraphWithRemovedEdge
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      var edge = graph.AddEdge(node.Index, node.Index, default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    }
  }

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNodes.graph.Nodes.First();
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdges.graph.Edges.First();
}