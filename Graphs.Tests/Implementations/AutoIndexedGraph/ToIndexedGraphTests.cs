using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

internal class ToIndexedGraphTests
{
  [Test]
  public void ToIndexedGraph_NodeDataIsTheSame()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var index in indices)
      graph.AddNode(index);

    // ACT
    var indexedGraph = graph.ToIndexedGraph();

    // ASSERT
    Assert.That(indexedGraph.Nodes.Select(node => node.Data), NUnit.Framework.Is.EquivalentTo(indices));
    foreach (var node in indexedGraph.Nodes)
      Assert.That(node.Index, NUnit.Framework.Is.EqualTo(node.Data));
  }

  [Test]
  public void ToIndexedGraph_EdgeDataIsTheSame()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var index = graph.AddNode(default).Index;
    foreach (var datum in data)
      graph.AddEdge(index, index, datum);

    // ACT
    var indexedGraph = graph.ToIndexedGraph();

    // ASSERT
    Assert.That(indexedGraph.Edges.Select(edge => edge.Data), NUnit.Framework.Is.EquivalentTo(data));
  }

  [Test]
  public void ToIndexedGraph_NodeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum);

    int CloneData(int input) => -input;

    // ACT
    var indexedGraph = graph.ToIndexedGraph(CloneData, n => n);

    // ASSERT
    var clonedNodeData = indexedGraph.Nodes.Select(node => node.Data);
    Assert.That(clonedNodeData, NUnit.Framework.Is.EquivalentTo(data.Select(CloneData)));
  }

  [Test]
  public void ToIndexedGraph_EdgeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    const int index1 = 0;
    const int index2 = 1;
    graph.AddNode(index1);
    graph.AddNode(index2);
    graph.AddEdge(index1, index1, data[0]);
    graph.AddEdge(index1, index2, data[1]);
    graph.AddEdge(index2, index1, data[2]);

    int CloneData(int input) => -input;

    // ACT
    var indexedGraph = graph.ToIndexedGraph(n => n, CloneData);

    // ASSERT
    var clonedEdgeData = indexedGraph.Edges.Select(node => node.Data);
    Assert.That(clonedEdgeData, NUnit.Framework.Is.EquivalentTo(data.Select(CloneData)));
  }

  [Test]
  public void ToIndexedGraph_StructureIsEquivalent()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    const int index1 = 0xC0FFEE;
    const int index2 = 0xBEEF;
    var edgeConnections = new[]
    {
      (index1, index1),
      (index1, index2),
      (index2, index1),
    };
    graph.AddNode(index1);
    graph.AddNode(index2);
    foreach (var edgeConnection in edgeConnections)
      graph.AddEdge(edgeConnection.Item1, edgeConnection.Item2, 0);

    // ACT
    var indexedGraph = graph.ToIndexedGraph();

    // ASSERT
    foreach (var edgeConnection in edgeConnections)
      Assert.That(indexedGraph.Edges.Count(edge =>
          edge.Origin.Index == edgeConnection.Item1 && edge.Destination.Index == edgeConnection.Item2),
        NUnit.Framework.Is.EqualTo(1));
  }
}