using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

internal class ToUnindexedGraphTests
{
  [Test]
  public void ToUnindexedGraph_NodeDataIsEquivalent()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum);

    // ACT
    var unindexedGraphs = new[]
    {
      graph.ToUnindexedGraph(),
      graph.ToUnindexedGraph(n => n, n => n),
    };

    // ASSERT
    foreach (var unindexedGraph in unindexedGraphs)
      Assert.That(unindexedGraph.Nodes.Select(node => node.Data), NUnit.Framework.Is.EquivalentTo(data));
  }

  [Test]
  public void ToUnindexedGraph_EdgeDataIsEquivalent()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var index = graph.AddNode(default).Index;
    foreach (var datum in data)
      graph.AddEdge(index, index, datum);

    // ACT
    var unindexedGraphs = new[]
    {
      graph.ToUnindexedGraph(),
      graph.ToUnindexedGraph(n => n, n => n),
    };

    // ASSERT
    foreach (var unindexedGraph in unindexedGraphs)
      Assert.That(unindexedGraph.Edges.Select(edge => edge.Data), NUnit.Framework.Is.EquivalentTo(data));
  }

  [Test]
  public void ToUnindexed_NodeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum);

    int CopyData(int input) => -input;

    // ACT
    var clonedGraph = graph.ToUnindexedGraph(CopyData, n => n);

    // ASSERT
    var clonedNodeData = clonedGraph.Nodes.Select(node => node.Data);
    Assert.That(clonedNodeData, NUnit.Framework.Is.EquivalentTo(data.Select(CopyData)));
  }

  [Test]
  public void ToUnindexed_EdgeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var index = graph.AddNode(default).Index;
    foreach (var datum in data)
      graph.AddEdge(index, index, datum);

    int CopyData(int input) => -input;

    // ACT
    var clonedGraph = graph.ToUnindexedGraph(n => n, CopyData);

    // ASSERT
    var clonedEdgeData = clonedGraph.Edges.Select(node => node.Data);
    Assert.That(clonedEdgeData, NUnit.Framework.Is.EquivalentTo(data.Select(CopyData)));
  }

  [Test]
  public void ToUnindexedGraph_StructureIsEquivalent()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new StatelessIndexProvider<int, int>(n => n));
    const int nodeData1 = 0xC0FFEE;
    const int nodeData2 = 0xBEEF;
    var index1 = graph.AddNode(nodeData1).Index;
    var index2 = graph.AddNode(nodeData2).Index;
    var edgeConnections = new[]
    {
      (index1, index1),
      (index1, index2),
      (index2, index1),
    };

    int MakeEdgeData(int origin, int destination) => origin + 2 * destination;

    foreach (var edgeConnection in edgeConnections)
      graph.AddEdge(edgeConnection.Item1, edgeConnection.Item2,
        MakeEdgeData(edgeConnection.Item1, edgeConnection.Item2));

    // ACT
    var clonedGraphs = new[]
    {
      graph.ToUnindexedGraph(),
      graph.ToUnindexedGraph(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
      Assert.That(clonedGraph.Edges.Select(edge => MakeEdgeData(edge.Origin.Data, edge.Destination.Data)),
        NUnit.Framework.Is.EquivalentTo(edgeConnections.Select(edgeConnection =>
          MakeEdgeData(edgeConnection.Item1, edgeConnection.Item2))));
  }
}