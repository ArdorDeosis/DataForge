using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests;

[TestFixture]
public class GraphTests
{
  [Test]
  public void EmptyGraph_HasNoNodes()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Nodes, Is.Empty);
  }

  [Test]
  public void EmptyGraph_HasNoEdges()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
    Assert.That((graph as IReadOnlyGraph<int, int>).Edges, Is.Empty);
  }

  [Test]
  public void EmptyGraph_OrderIsZero()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ASSERT
    Assert.That(graph.Order, Is.Zero);
  }

  [Test]
  public void EmptyGraph_SizeIsZero()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ASSERT
    Assert.That(graph.Size, Is.Zero);
  }

  [Test]
  public void Order_HasExpectedValue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddNode(0);
    graph.AddNode(0);
    graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Order, Is.EqualTo(3));
  }

  [Test]
  public void Size_HasExpectedValue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);
    graph.AddEdge(node, node, 0);
    graph.AddEdge(node, node, 0);
    graph.AddEdge(node, node, 0);

    // ASSERT
    Assert.That(graph.Size, Is.EqualTo(3));
  }

  [Test]
  public void AddNode_Node_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(data);

    // ASSERT
    Assert.That(node.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddNode_Graph_ContainsAddedNode()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }

  [Test]
  public void AddNodes_Nodes_HaveExpectedData()
  {
    // ARRANGE
    var data = new[] { 0xC0FFEE, 0xBEEF };
    var graph = new Graph<int, int>();

    // ACT
    var nodes = graph.AddNodes(data).ToList();

    // ASSERT
    Assert.That(nodes, Has.Count.EqualTo(data.Length));
    Assert.That(nodes.Select(node => node.Data), Is.EquivalentTo(data));
  }

  [Test]
  public void AddNodes_Graph_ContainsAddedNodes()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ACT
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.Contains(node));
    Assert.That(graph.Nodes.Contains(node));
  }

  [Test]
  public void AddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    var edge = graph.AddEdge(origin, destination, data);

    // ASSERT
    Assert.That(edge.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void AddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ASSERT
    Assert.That(graph.Contains(edge));
    Assert.That(graph.Edges.Contains(edge));
  }


  [Test]
  public void AddEdge_WrongNode_ThrowsArgumentException()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var validNode = graph.AddNode(0);
    var invalidNode = new Graph<int, int>().AddNode(0);
    var removedNode = graph.AddNode(0);
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(() => graph.AddEdge(validNode, invalidNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(validNode, removedNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(invalidNode, validNode, 0), Throws.ArgumentException);
    Assert.That(() => graph.AddEdge(removedNode, validNode, 0), Throws.ArgumentException);
  }

  [Test]
  public void TryAddEdge_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.TryAddEdge(origin, destination, 0, out _));
  }

  [Test]
  public void TryAddEdge_Edge_HasExpectedData()
  {
    // ARRANGE
    const int data = 0xC0FFEE;
    var graph = new Graph<int, int>();
    var origin = graph.AddNode(0);
    var destination = graph.AddNode(0);

    // ACT
    graph.TryAddEdge(origin, destination, data, out var edge);

    // ASSERT
    Assert.That(edge, Is.Not.Null);
    Assert.That(edge!.Origin, Is.EqualTo(origin));
    Assert.That(edge.Destination, Is.EqualTo(destination));
    Assert.That(edge.Data, Is.EqualTo(data));
  }

  [Test]
  public void TryAddEdge_Graph_ContainsAddedEdges()
  {
    // ARRANGE
    var graph = new Graph<int, int>();

    // ACT
    graph.TryAddEdge(graph.AddNode(0), graph.AddNode(0), 0, out var edge);

    // ASSERT
    Assert.That(graph.Contains(edge!));
    Assert.That(graph.Edges.Contains(edge));
  }

  [Test]
  public void TryAddEdge_WrongNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var validNode = graph.AddNode(0);
    var invalidNode = new Graph<int, int>().AddNode(0);
    var removedNode = graph.AddNode(0);
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(graph.TryAddEdge(validNode, invalidNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(validNode, removedNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(invalidNode, validNode, 0, out _), Is.False);
    Assert.That(graph.TryAddEdge(removedNode, validNode, 0, out _), Is.False);
  }

  [Test]
  public void RemoveNode_NodeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);

    // ASSERT
    Assert.That(graph.RemoveNode(node), Is.True);
  }

  [Test]
  public void RemoveNode_NodeInGraph_NodeIsInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void RemoveNode_NodeInGraph_GraphDoesNotContainNode()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Contains(node), Is.False);
    Assert.That(graph.Nodes.Contains(node), Is.False);
  }

  [Test]
  public void RemoveNode_InvalidNode_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var nodeInOtherGraph = new Graph<int, int>().AddNode(0);
    var removedNode = graph.AddNode(0);
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(graph.RemoveNode(nodeInOtherGraph), Is.False);
    Assert.That(graph.RemoveNode(removedNode), Is.False);
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);
    var edge1 = graph.AddEdge(node, graph.AddNode(0), 0);
    var edge2 = graph.AddEdge(graph.AddNode(0), node, 0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(graph.Edges, Does.Not.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void RemoveNode_ConnectedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);
    var edge1 = graph.AddEdge(node, graph.AddNode(0), 0);
    var edge2 = graph.AddEdge(graph.AddNode(0), node, 0);

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(edge1.IsValid, Is.False);
    Assert.That(edge2.IsValid, Is.False);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_ReturnsTrue()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ASSERT
    Assert.That(graph.RemoveEdge(edge), Is.True);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_EdgeIsInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(edge.IsValid, Is.False);
  }

  [Test]
  public void RemoveEdge_EdgeInGraph_GraphDoesNotContainEdge()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(graph.Contains(edge), Is.False);
    Assert.That(graph.Edges.Contains(edge), Is.False);
  }

  [Test]
  public void RemoveEdge_InvalidEdge_ReturnsFalse()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var otherGraph = new Graph<int, int>();
    var edgeInOtherGraph = otherGraph.AddEdge(otherGraph.AddNode(0), otherGraph.AddNode(0), 0);
    var removedEdge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);
    graph.RemoveEdge(removedEdge);

    // ASSERT
    Assert.That(graph.RemoveEdge(edgeInOtherGraph), Is.False);
    Assert.That(graph.RemoveEdge(removedEdge), Is.False);
  }

  [Test]
  public void RemoveNodesWhere_ExpectedNodesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node1 = graph.AddNode(-1);
    var node2 = graph.AddNode(1);

    // ACT
    graph.RemoveNodesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Nodes, Does.Contain(node1));
    Assert.That(graph.Nodes, Does.Not.Contain(node2));
  }

  [Test]
  public void RemoveEdgesWhere_ExpectedEdgesAreRemoved()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge1 = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), -1);
    var edge2 = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 1);

    // ACT
    graph.RemoveEdgesWhere(data => data > 0);

    // ASSERT
    Assert.That(graph.Contains(edge1), Is.True);
    Assert.That(graph.Edges, Does.Contain(edge1));
    Assert.That(graph.Edges, Does.Not.Contain(edge2));
  }

  [Test]
  public void Clear_RemovesNodes()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddNode(0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Nodes, Is.Empty);
  }

  [Test]
  public void Clear_RemovesEdges()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(graph.Edges, Is.Empty);
  }

  [Test]
  public void Clear_RemovedNodesAreInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var node = graph.AddNode(0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(node.IsValid, Is.False);
  }

  [Test]
  public void Clear_RemovedEdgesAreInvalid()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    var edge = graph.AddEdge(graph.AddNode(0), graph.AddNode(0), 0);

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(edge.IsValid, Is.False);
  }

  [Test]
  public void Clone_DataIsEqual()
  {
    // ARRANGE
    var graph = new Graph<object, object>();
    graph.AddEdge(graph.AddNode(new { }), graph.AddNode(new { }), new { });

    // ACT
    var clonedGraph = graph.Clone();

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(graph.Nodes.Select(node => node.Data)));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(graph.Edges.Select(edge => edge.Data)));
  }

  [Test]
  public void Clone_StructureIsEqual()
  {
    // ARRANGE
    const string originNodeData = nameof(originNodeData);
    const string middleNodeData = nameof(middleNodeData);
    const string destinationNodeData = nameof(destinationNodeData);
    const string firstEdgeData = nameof(firstEdgeData);
    const string secondEdgeData = nameof(secondEdgeData);
    var graph = new Graph<string, string>();
    graph.AddEdge(graph.AddNode(originNodeData), graph.AddNode(middleNodeData), firstEdgeData);
    graph.AddEdge(graph.AddNode(middleNodeData), graph.AddNode(destinationNodeData), secondEdgeData);

    // ACT
    var clonedGraph = graph.Clone();

    // ASSERT
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is originNodeData &&
      edge.Destination.Data is middleNodeData &&
      edge.Data is firstEdgeData).ToList(), Has.Count.EqualTo(1));
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is middleNodeData &&
      edge.Destination.Data is destinationNodeData &&
      edge.Data is secondEdgeData).ToList(), Has.Count.EqualTo(1));
  }

  [Test]
  public void Clone_DataCloningFunctions_WorkAsExpected()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(1), graph.AddNode(2), 3);

    // ACT
    var clonedGraph = graph.Clone(data => -data, data => -data);

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { -1, -2 }));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(new[] { -3 }));
  }

  [Test]
  public void CloneAndTransform_DataTransformationFunctions_WorkAsExpected()
  {
    // ARRANGE
    var graph = new Graph<int, int>();
    graph.AddEdge(graph.AddNode(1), graph.AddNode(2), 3);

    // ACT
    var clonedGraph = graph.CloneAndTransform(data => data.ToString(), data => data.ToString());

    // ASSERT
    Assert.That(clonedGraph.Nodes.Select(node => node.Data), Is.EquivalentTo(new[] { "1", "2" }));
    Assert.That(clonedGraph.Edges.Select(edge => edge.Data), Is.EquivalentTo(new[] { "3" }));
  }


  [Test]
  public void CloneAndTransform_StructureIsEqual()
  {
    // ARRANGE
    const string originNodeData = nameof(originNodeData);
    const string middleNodeData = nameof(middleNodeData);
    const string destinationNodeData = nameof(destinationNodeData);
    const string firstEdgeData = nameof(firstEdgeData);
    const string secondEdgeData = nameof(secondEdgeData);
    var graph = new Graph<string, string>();
    graph.AddEdge(graph.AddNode(originNodeData), graph.AddNode(middleNodeData), firstEdgeData);
    graph.AddEdge(graph.AddNode(middleNodeData), graph.AddNode(destinationNodeData), secondEdgeData);

    // ACT
    var clonedGraph = graph.CloneAndTransform(data => data, data => data);

    // ASSERT
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is originNodeData &&
      edge.Destination.Data is middleNodeData &&
      edge.Data is firstEdgeData).ToList(), Has.Count.EqualTo(1));
    Assert.That(clonedGraph.Edges.Where(edge =>
      edge.Origin.Data is middleNodeData &&
      edge.Destination.Data is destinationNodeData &&
      edge.Data is secondEdgeData).ToList(), Has.Count.EqualTo(1));
  }
}