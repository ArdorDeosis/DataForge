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
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    for (var index = 0; index < data.Length; index++)
      graph.AddNode(index, data[index]);
    int TransformData(int data) => -data;

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(TransformData, n => n);

    // ASSERT
    Assert.That(unindexedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(data.Select(TransformData)));
  }

  [Test]
  public void TransformToUnindexedGraph_EdgeDataIsTransformedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    var indices = new[] { 0, 1 };
    graph.AddNode(indices[0], 0);
    graph.AddNode(indices[1], 0);
    graph.AddEdge(indices[0], indices[1], data[0]);
    graph.AddEdge(indices[0], indices[1], data[1]);
    graph.AddEdge(indices[0], indices[1], data[2]);
    int TransformData(int data) => -data;

    // ACT
    var unindexedGraph = graph.TransformToUnindexedGraph(n => n, TransformData);

    // ASSERT
    Assert.That(unindexedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(data.Select(TransformData)));
  }


  [Test]
  public void TransformToUnindexedGraph_StructureIsEquivalent()
  {
    //TODO
  }
}