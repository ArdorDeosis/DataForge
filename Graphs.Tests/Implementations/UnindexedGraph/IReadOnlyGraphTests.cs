using System.Linq;
using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.UnindexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IReadOnlyGraphTests : IReadOnlyGraphTests<Graph<int, int>>
{
  protected override Graph<int, int> GetEmptyGraph => new();

  protected override (Graph<int, int> graph, INode<int, int>[] expectedNodes) GraphWithNodes
  {
    get
    {
      var graph = new Graph<int, int>();
      var nodes = new INode<int, int>[]
      {
        graph.AddNode(default),
        graph.AddNode(default),
        graph.AddNode(default),
      };
      return (graph, nodes);
    }
  }

  protected override (Graph<int, int> graph, IEdge<int, int>[] expectedEdges) GraphWithEdges
  {
    get
    {
      var graph = new Graph<int, int>();
      var edges = new IEdge<int, int>[]
      {
        graph.AddEdge(graph.AddNode(default), graph.AddNode(default), default),
        graph.AddEdge(graph.AddNode(default), graph.AddNode(default), default),
      };
      return (graph, edges);
    }
  }

  protected override (Graph<int, int> graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new Graph<int, int>();
      var node = graph.AddNode(default);
      graph.RemoveNode(node);
      return (graph, node);
    }
  }

  protected override (Graph<int, int> graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge
  {
    get
    {
      var graph = new Graph<int, int>();
      var edge = graph.AddEdge(graph.AddNode(default), graph.AddNode(default), default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    }
  }

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNodes.graph.Nodes.First();
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdges.graph.Edges.First();
}