using DataForge.Graphs.Tests.Interfaces;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.UnindexedGraph;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class IUnindexedGraphTests : IUnindexedGraphTests<Graph<int, int>>
{
  protected override Graph<int, int> EmptyGraph => new();

  protected override (Graph<int, int> graph, Node<int, int> removedNode) EmptyGraphWithRemovedNode
  {
    get
    {
      var graph = new Graph<int, int>();
      var node = graph.AddNode(default);
      graph.RemoveNode(node);
      return (graph, node);
    }
  }

  protected override Node<int, int> NodeFromOtherGraph => new Graph<int, int>().AddNode(default);
}