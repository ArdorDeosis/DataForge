using DataForge.Graphs;
using DataForge.Graphs.Observable;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IGraphTests : IGraphTests<ObservableIndexedGraph<int, int, int>>
{
  protected override (ObservableIndexedGraph<int, int, int> graph, INode<int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new ObservableIndexedGraph<int, int, int>();
      var node = graph.AddNode(0xC0FFEE, default);
      return (graph, node);
    }
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IReadOnlyCollection<INode<int, int>> expectedNodes)
    GraphWithNodes
  {
    get
    {
      var graph = new ObservableIndexedGraph<int, int, int>();
      return (graph, new[]
      {
        graph.AddNode(0xC0FFEE, default),
        graph.AddNode(0xBEEF, default),
        graph.AddNode(0xF00D, default),
      });
    }
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IReadOnlyCollection<INode<int, int>> expectedNodes)
    GraphWithNodesWithData(IReadOnlyCollection<int> data)
  {
    var counter = 0;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var nodes = data.Select(datum => graph.AddNode(counter++, datum));
    return (graph, nodes.ToArray());
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IEdge<int, int> expectedEdge) GraphWithEdge
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

  protected override (ObservableIndexedGraph<int, int, int> graph, IReadOnlyCollection<IEdge<int, int>> expectedEdges)
    GraphWithEdges
  {
    get
    {
      const int index1 = 0xC0FFEE;
      const int index2 = 0xF00D;
      var graph = new ObservableIndexedGraph<int, int, int>();
      graph.AddNode(index1, default);
      graph.AddNode(index2, default);
      var edges = new[]
      {
        graph.AddEdge(index1, index1, default),
        graph.AddEdge(index1, index2, default),
        graph.AddEdge(index2, index1, default),
      };
      return (graph, edges);
    }
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, IReadOnlyCollection<IEdge<int, int>> expectedEdges)
    GraphWithEdgesWithData(IReadOnlyCollection<int> data)
  {
    var counter = 0;
    var graph = new ObservableIndexedGraph<int, int, int>();
    var edges = data.Select(datum => graph.AddEdge(
      graph.AddNode(counter++, default).Index,
      graph.AddNode(counter++, default).Index,
      datum)
    );
    return (graph, edges.ToArray());
  }

  protected override (ObservableIndexedGraph<int, int, int> graph, INode<int, int> removedNode)
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

  protected override (ObservableIndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge)
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

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}