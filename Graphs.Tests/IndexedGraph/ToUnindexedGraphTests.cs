using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

internal class ToUnindexedGraphTests
{
  [Test]
  public void ToUnindexedGraph_NodeDataIsEquivalent()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    for (var index = 0; index < data.Length; index++)
      graph.AddNode(index, data[index]);

    // ACT
    var unindexedGraphs = new[]
    {
      graph.ToUnindexedGraph(),
      graph.ToUnindexedGraph(n => n, n => n),
    };

    // ASSERT
    foreach (var unindexedGraph in unindexedGraphs)
      Assert.That(unindexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(data));
  }

  [Test]
  public void ToUnindexedGraph_EdgeDataIsEquivalent()
  {
    // TODO
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var indices = new[] { 0, 1 };
    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    graph.AddEdge(indices[0], indices[1], data[0]);
    graph.AddEdge(indices[0], indices[1], data[1]);
    graph.AddEdge(indices[0], indices[1], data[2]);

    // ACT
    var unindexedGraphs = new[]
    {
      graph.ToUnindexedGraph(),
      graph.ToUnindexedGraph(n => n, n => n)
    };

    // ASSERT
    foreach (var unindexedGraph in unindexedGraphs)
      Assert.That(unindexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(data));
  }

  [Test]
  public void ToUnindexed_NodeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum, datum);
    int CopyData(int input) => -input;

    // ACT
    var clonedGraph = graph.ToUnindexedGraph(CopyData, n => n);

    // ASSERT
    var clonedNodeData = clonedGraph.Nodes.Select(node => node.Data);
    Assert.That(clonedNodeData, Is.EquivalentTo(data.Select(CopyData)));
  }

  [Test]
  public void ToUnindexed_EdgeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    const int index1 = 0;
    const int index2 = 1;
    graph.AddNode(index1, 0);
    graph.AddNode(index2, 0);
    graph.AddEdge(index1, index1, data[0]);
    graph.AddEdge(index1, index2, data[1]);
    graph.AddEdge(index2, index1, data[2]);
    int CopyData(int input) => -input;

    // ACT
    var clonedGraph = graph.ToUnindexedGraph(n => n, CopyData);

    // ASSERT
    var clonedEdgeData = clonedGraph.Edges.Select(node => node.Data);
    Assert.That(clonedEdgeData, Is.EquivalentTo(data.Select(CopyData)));
  }

  [Test]
  public void ToUnindexedGraph_StructureIsEquivalent()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int nodeData1 = 0xC0FFEE;
    const int nodeData2 = 0xBEEF;
    var edgeConnections = new[]
    {
      (nodeData1, nodeData1),
      (nodeData1, nodeData2),
      (nodeData2, nodeData1),
    };
    graph.AddNode(nodeData1, nodeData1);
    graph.AddNode(nodeData2, nodeData2);
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
        Is.EquivalentTo(edgeConnections.Select(edgeConnection =>
          MakeEdgeData(edgeConnection.Item1, edgeConnection.Item2))));
  }
}