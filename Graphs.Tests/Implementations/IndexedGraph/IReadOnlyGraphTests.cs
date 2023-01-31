using System.Linq;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IReadOnlyGraphTests : IReadOnlyGraphTests<IndexedGraph<int, int, int>>
{
  protected override IndexedGraph<int, int, int> GetEmptyGraph => new();

  protected override (IndexedGraph<int, int, int> graph, INode<int, int>[] expectedNodes) GraphWithNodes
  {
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      var nodes = new INode<int, int>[]
      {
        graph.AddNode(0, default),
        graph.AddNode(1, default),
        graph.AddNode(2, default),
      };
      return (graph, nodes);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, IEdge<int, int>[] expectedEdges) GraphWithEdges
  {
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      var node1 = graph.AddNode(0, default);
      var node2 = graph.AddNode(1, default);
      var edges = new IEdge<int, int>[]
      {
        graph.AddEdge(node1.Index, node1.Index, default),
        graph.AddEdge(node1.Index, node2.Index, default),
        graph.AddEdge(node2.Index, node1.Index, default),
      };
      return (graph, edges);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      const int index = 0;
      var node = graph.AddNode(index, default);
      graph.RemoveNode(index);
      return (graph, node);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge { 
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      var node = graph.AddNode(0, default);
      var edge = graph.AddEdge(node.Index, node.Index, default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    } }

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNodes.graph.Nodes.First();
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdges.graph.Edges.First();
}