using DataForge.Graphs;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IGraphTests : IGraphTests<ObservableAutoIndexedGraph<int, int, int>>
{
  protected override (ObservableAutoIndexedGraph<int, int, int> graph, INode<int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IReadOnlyCollection<INode<int, int>>
    expectedNodes)
    GraphWithNodes
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      return (graph, new[]
      {
        graph.AddNode(default),
        graph.AddNode(default),
        graph.AddNode(default),
      });
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IReadOnlyCollection<INode<int, int>>
    expectedNodes)
    GraphWithNodesWithData(IReadOnlyCollection<int> data)
  {
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var nodes = data.Select(datum => graph.AddNode(datum));
    return (graph, nodes.ToArray());
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IEdge<int, int> expectedEdge) GraphWithEdge
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default);
      return (graph, edge);
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IReadOnlyCollection<IEdge<int, int>>
    expectedEdges)
    GraphWithEdges
  {
    get
    {
      var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      return (graph, new[]
      {
        graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default),
        graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default),
        graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default),
      });
    }
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IReadOnlyCollection<IEdge<int, int>>
    expectedEdges)
    GraphWithEdgesWithData(IReadOnlyCollection<int> data)
  {
    var graph = new ObservableAutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var edges = data.Select(datum => graph.AddEdge(
      graph.AddNode(default).Index,
      graph.AddNode(default).Index,
      datum)
    );
    return (graph, edges.ToArray());
  }

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, INode<int, int> removedNode)
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

  protected override (ObservableAutoIndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge)
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

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}