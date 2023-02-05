using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IGraphTests : IGraphTests<AutoIndexedGraph<int, int, int>>
{
  protected override (AutoIndexedGraph<int, int, int> graph, INode<int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      return (graph, node);
    }
  }

  protected override (AutoIndexedGraph<int, int, int> graph, IEdge<int, int> expectedEdge) GraphWithEdge
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default);
      return (graph, edge);
    }
  }

  protected override (AutoIndexedGraph<int, int, int> graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var node = graph.AddNode(default);
      graph.RemoveNode(node);
      return (graph, node);
    }
  }

  protected override (AutoIndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge
  {
    get
    {
      var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
      var edge = graph.AddEdge(graph.AddNode(default).Index, graph.AddNode(default).Index, default);
      graph.RemoveEdge(edge);
      return (graph, edge);
    }
  }

  protected override INode<int, int> NodeFromOtherGraph => GraphWithNode.expectedNode;
  protected override IEdge<int, int> EdgeFromOtherGraph => GraphWithEdge.expectedEdge;
}