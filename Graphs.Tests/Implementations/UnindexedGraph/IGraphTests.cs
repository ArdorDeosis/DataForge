using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.UnindexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IGraphTests : IGraphTests<Graph<int, int>>
{
  protected override (Graph<int, int> graph, INode<int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new Graph<int, int>();
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }

  protected override (Graph<int, int> graph, IEdge<int, int> expectedEdge) GraphWithEdge
  {
    get
    {
      var graph = new Graph<int, int>();
      var edge = graph.AddEdge(graph.AddNode(default), graph.AddNode(default), default);
      return (graph, edge);
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

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}