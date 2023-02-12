using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.AutoIndexedGraph;

internal class TransformToUnindexedGraphTests
{
  [Test]
  public void TransformToUnindexedGraph_NodeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var nodeData = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in nodeData)
      graph.AddNode(datum);

    string TransformData(int data) => data.ToString();

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(TransformData, n => n);

    // ASSERT
    Assert.That(unindexedGraph.Nodes.Select(node => node.Data),
      NUnit.Framework.Is.EquivalentTo(nodeData.Select(TransformData)));
  }

  [Test]
  public void TransformToUnindexedGraph_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new AutoIndexedGraph<int, int, int>(new IncrementalIndexProvider<int, int>(0));
    var edgeData = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var index = graph.AddNode(default).Index;
    graph.AddEdge(index, index, edgeData[0]);
    graph.AddEdge(index, index, edgeData[1]);
    graph.AddEdge(index, index, edgeData[2]);

    string TransformData(int data) => data.ToString();

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(n => n, TransformData);

    // ASSERT
    Assert.That(unindexedGraph.Edges.Select(edge => edge.Data),
      NUnit.Framework.Is.EquivalentTo(edgeData.Select(TransformData)));
  }


  [Test]
  public void TransformToUnindexedGraph_StructureIsEquivalent()
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