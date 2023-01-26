﻿using System.Linq;
using NUnit.Framework;

namespace DataForge.Graphs.Tests.UnindexedGraph;

public class RemoveNodeTests
{
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
}