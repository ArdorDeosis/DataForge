using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

internal class TransformTests
{
  [Test]
  public void Transform_IndicesAreTransformed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var index in indices)
      graph.AddNode(index, index);

    // ACT
    var transformedGraphs = new[]
    {
      graph.Transform(n => n, n => n, n => n),
    };

    // ASSERT
    foreach (var transformedGraph in transformedGraphs)
    {
      Assert.That(transformedGraph.Indices, NUnit.Framework.Is.EquivalentTo(indices));
      Assert.That(transformedGraph.Indices, NUnit.Framework.Is.EquivalentTo(graph.Indices));
    }
  }

  [Test]
  public void Transform_WithEqualityComparer_EqualityComparerIsUsedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var (index, equivalentIndex) = TestEqualityComparer.EquivalentIndexPair;
    graph.AddNode(index, 0);

    // ACT
    var transformedGraphs = new[]
    {
      graph.Transform(n => n, n => n, n => n, new TestEqualityComparer()),
      graph.Transform(n => n, n => n, n => n, () => new TestEqualityComparer()),
    };

    // ASSERT
    foreach (var transformedGraph in transformedGraphs)
      Assert.That(() => transformedGraph.AddNode(equivalentIndex, equivalentIndex), Throws.InvalidOperationException);
  }

  // [Test]
  // public void Transform_NodeDataTransformFunctionIsUsed()
  // {
  //   // ARRANGE
  //   var graph = new IndexedGraph<int, int, int>();
  //   var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
  //   foreach (var datum in data)
  //     graph.AddNode(datum, datum);
  //   int TransformData(int input) => -input;
  //
  //   // ACT
  //   var transformedGraph = graph.Transform(TransformData, n => n);
  //
  //   // ASSERT
  //   var transformedNodeData = transformedGraph.Nodes.Select(node => node.Data);
  //   Assert.That(transformedNodeData, Is.EquivalentTo(data.Select(TransformData)));
  // }
  //
  // [Test]
  // public void Transform_EdgeDataTransformFunctionIsUsed()
  // {
  //   // ARRANGE
  //   var graph = new IndexedGraph<int, int, int>();
  //   var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
  //   const int index1 = 0;
  //   const int index2 = 1;
  //   graph.AddNode(index1, 0);
  //   graph.AddNode(index2, 0);
  //   graph.AddEdge(index1, index1, data[0]);
  //   graph.AddEdge(index1, index2, data[1]);
  //   graph.AddEdge(index2, index1, data[2]);
  //   int TransformData(int input) => -input;
  //
  //   // ACT
  //   var transformedGraph = graph.Transform(n => n, TransformData);
  //
  //   // ASSERT
  //   var transformedEdgeData = transformedGraph.Edges.Select(node => node.Data);
  //   Assert.That(transformedEdgeData, Is.EquivalentTo(data.Select(TransformData)));
  // }

  [Test]
  public void Transform_StructureIsEquivalent()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    const int index1 = 0xC0FFEE;
    const int index2 = 0xBEEF;
    var edgeConnections = new[]
    {
      (index1, index1),
      (index1, index2),
      (index2, index1),
    };
    graph.AddNode(index1, 0);
    graph.AddNode(index2, 0);
    foreach (var edgeConnection in edgeConnections)
      graph.AddEdge(edgeConnection.Item1, edgeConnection.Item2, 0);

    string TransformData(int data) => data.ToString();

    // ACT
    var transformedGraphs = new[]
    {
      graph.Transform(TransformData, TransformData, TransformData),
    };

    // ASSERT
    foreach (var transformedGraph in transformedGraphs)
      foreach (var edgeConnection in edgeConnections)
        Assert.That(transformedGraph.Edges.Count(edge =>
            edge.OriginIndex == TransformData(edgeConnection.Item1) &&
            edge.DestinationIndex == TransformData(edgeConnection.Item2)),
          NUnit.Framework.Is.EqualTo(1));
  }
}