using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

internal class TransformToIndexedGraphTests
{
  [Test]
  public void TransformToIndexedGraph_IndexCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(n => n);
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var index in indices)
      graph.AddNode(index);
    string TransformData(int input) => input.ToString();

    // ACT
    var indexedGraph = graph.TransformToIndexedGraph(n => n, n => n, TransformData);

    // ASSERT
    Assert.That(indexedGraph.Indices, Is.EquivalentTo(indices.Select(TransformData)));
  }

  [Test]
  public void TransformToIndexedGraph_NodeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum);
    string TransformData(int input) => input.ToString();

    // ACT
    var indexedGraph = graph.TransformToIndexedGraph(TransformData, n => n, n => n);

    // ASSERT
    var clonedNodeData = indexedGraph.Nodes.Select(node => node.Data);
    Assert.That(clonedNodeData, Is.EquivalentTo(data.Select(TransformData)));
  }

  [Test]
  public void TransformToIndexedGraph_EdgeDataCloneFunctionIsUsed()
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
    string CloneData(int input) => input.ToString();

    // ACT
    var indexedGraph = graph.TransformToIndexedGraph(n => n, CloneData, n => n);

    // ASSERT
    var clonedEdgeData = indexedGraph.Edges.Select(node => node.Data);
    Assert.That(clonedEdgeData, Is.EquivalentTo(data.Select(CloneData)));
  }

  [Test]
  public void TransformToIndexedGraph_StructureIsEquivalent()
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
    var indexedGraph = graph.TransformToIndexedGraph(n => n, n => n, n => n);

    // ASSERT
    foreach (var edgeConnection in edgeConnections)
    {
      Assert.That(indexedGraph.Edges.Count(edge =>
          edge.Origin.Index == edgeConnection.Item1 && edge.Destination.Index == edgeConnection.Item2),
        Is.EqualTo(1));
    }
  }
}