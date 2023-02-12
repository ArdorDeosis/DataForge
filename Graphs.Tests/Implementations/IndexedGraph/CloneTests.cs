using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.Implementations.IndexedGraph;

internal class CloneTests
{
  [Test]
  public void Clone_IndicesAreEqual()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var index in indices)
      graph.AddNode(index, index);

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
    {
      Assert.That(clonedGraph.Indices, NUnit.Framework.Is.EquivalentTo(indices));
      Assert.That(clonedGraph.Indices, NUnit.Framework.Is.EquivalentTo(graph.Indices));
    }
  }

  [Test]
  public void Clone_WithEqualityComparerInstance_EqualityComparerIsClonedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>(new TestEqualityComparer());
    var (index, equivalentIndex) = TestEqualityComparer.EquivalentIndexPair;
    graph.AddNode(index, 0);

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
      Assert.That(() => clonedGraph.AddNode(equivalentIndex, equivalentIndex), Throws.InvalidOperationException);
  }

  [Test]
  public void Clone_WithEqualityComparerFactoryMethod_EqualityComparerIsClonedCorrectly()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>(() => new TestEqualityComparer());
    var (index, equivalentIndex) = TestEqualityComparer.EquivalentIndexPair;
    graph.AddNode(index, 0);

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
      Assert.That(() => clonedGraph.AddNode(equivalentIndex, equivalentIndex), Throws.InvalidOperationException);
  }

  [Test]
  public void Clone_NodeDataIsEqual()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum, datum);

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
    {
      var clonedNodeData = clonedGraph.Nodes.Select(node => node.Data);
      Assert.That(clonedNodeData, NUnit.Framework.Is.EquivalentTo(data));
    }
  }

  [Test]
  public void Clone_EdgeDataIsEqual()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var indices = new[] { 0, 1 };
    var edgeData = new[] { 0xC0FFEE, 0xBEEF };
    foreach (var index in indices)
      graph.AddNode(index, 0);
    graph.AddEdge(indices[0], indices[1], edgeData[0]);
    graph.AddEdge(indices[1], indices[0], edgeData[1]);

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
    {
      var clonedEdgeData = clonedGraph.Edges.Select(node => node.Data);
      Assert.That(clonedEdgeData, NUnit.Framework.Is.EquivalentTo(edgeData));
    }
  }

  [Test]
  public void Clone_NodeDataCloneFunctionIsUsed()
  {
    // ARRANGE
    var graph = new IndexedGraph<int, int, int>();
    var data = new[] { 0xC0FFEE, 0xBEEF, 0xF00D };
    foreach (var datum in data)
      graph.AddNode(datum, datum);

    int CloneData(int input) => -input;

    // ACT
    var clonedGraph = graph.Clone(CloneData, n => n);

    // ASSERT
    var clonedNodeData = clonedGraph.Nodes.Select(node => node.Data);
    Assert.That(clonedNodeData, NUnit.Framework.Is.EquivalentTo(data.Select(CloneData)));
  }

  [Test]
  public void Clone_EdgeDataCloneFunctionIsUsed()
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

    int CloneData(int input) => -input;

    // ACT
    var clonedGraph = graph.Clone(n => n, CloneData);

    // ASSERT
    var clonedEdgeData = clonedGraph.Edges.Select(node => node.Data);
    Assert.That(clonedEdgeData, NUnit.Framework.Is.EquivalentTo(data.Select(CloneData)));
  }

  [Test]
  public void Clone_StructureIsEquivalent()
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

    // ACT
    var clonedGraphs = new[]
    {
      graph.Clone(),
      graph.Clone(n => n, n => n),
    };

    // ASSERT
    foreach (var clonedGraph in clonedGraphs)
      foreach (var edgeConnection in edgeConnections)
        Assert.That(clonedGraph.Edges.Count(edge =>
            edge.OriginIndex == edgeConnection.Item1 && edge.DestinationIndex == edgeConnection.Item2),
          NUnit.Framework.Is.EqualTo(1));
  }
}