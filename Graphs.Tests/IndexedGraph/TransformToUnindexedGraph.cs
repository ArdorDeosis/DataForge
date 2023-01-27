using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.IndexedGraph;

[TestFixture]
public class TransformToUnindexedGraph
{
  [Test]
  public void TransformToUnindexedGraph_NodeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var nodeData = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    for (var index = 0; index < nodeData.Length; index++)
      graph.AddNode(index, nodeData[index]);
    string TransformData(int data) => data.ToString();

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(TransformData, n => n);

    // ASSERT
    Assert.That(unindexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(nodeData.Select(TransformData)));
  }

  [Test]
  public void TransformToUnindexedGraph_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var edgeData = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var indices = new[] { 0, 1 };
    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    graph.AddEdge(indices[0], indices[1], edgeData[0]);
    graph.AddEdge(indices[0], indices[1], edgeData[1]);
    graph.AddEdge(indices[0], indices[1], edgeData[2]);
    string TransformData(int data) => data.ToString();

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(n => n, TransformData);

    // ASSERT
    Assert.That(unindexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(edgeData.Select(TransformData)));
  }


  [Test]
  public void TransformToUnindexedGraph_StructureIsEquivalent()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var nodeData = new[] { 0xC0FFEE, 0xBEEF };
    var edgeConnections = new[]
    {
      (nodeData[0], nodeData[0]),
      (nodeData[0], nodeData[1]),
      (nodeData[1], nodeData[0]),
    };
    graph.AddNode(nodeData[0], nodeData[0]);
    graph.AddNode(nodeData[1], nodeData[1]);
    for (var index = 0; index < edgeConnections.Length; index++)
      graph.AddEdge(edgeConnections[index].Item1, edgeConnections[index].Item2, index);

    string TransformData(int data) => data.ToString();

    // ACT
    var transformedGraph = graph.TransformToUnindexedGraph(TransformData, TransformData);

    // ASSERT
    for (var index = 0; index < edgeConnections.Length; index++)
    {
      var correspondingEdges = transformedGraph.Edges
        .Where(edge =>
          edge.Origin.Data == TransformData(edgeConnections[index].Item1) &&
          edge.Destination.Data == TransformData(edgeConnections[index].Item2) &&
          edge.Data == TransformData(index))
        .ToArray();
      Assert.That(correspondingEdges, Has.Length.EqualTo(1));
    }
  }
}