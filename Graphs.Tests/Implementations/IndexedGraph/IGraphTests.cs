using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IGraphTests : IGraphTests<IndexedGraph<int, int, int>>
{
  protected override (IndexedGraph<int, int, int> graph, INode<int, int> expectedNode) GraphWithNode
  {
    get
    {
      var graph = new IndexedGraph<int, int, int>();
      var node = graph.AddNode(0xC0FFEE, default);
      return (graph, node);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, IEdge<int, int> expectedEdge) GraphWithEdge
  {
    get
    {
      const int index1 = 0xC0FFEE;
      const int index2 = 0xF00D;
      var graph = new IndexedGraph<int, int, int>();
      graph.AddNode(index1, default);
      graph.AddNode(index2, default);
      var edge = graph.AddEdge(index1, index2, default);
      return (graph, edge);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, INode<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      const int index = 0xC0FFEE;
      var graph = new IndexedGraph<int, int, int>();
      var node = graph.AddNode(index, default);
      graph.RemoveNode(index);
      return (graph, node);
    }
  }

  protected override (IndexedGraph<int, int, int> graph, IEdge<int, int> removedEdge) EmptyGraphWithRemovedEdge
  {
    get
    {
      const int index1 = 0xC0FFEE;
      const int index2 = 0xF00D;
      var graph = new IndexedGraph<int, int, int>();
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