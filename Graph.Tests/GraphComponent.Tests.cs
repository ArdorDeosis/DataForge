using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Graph.Tests;

public class GraphComponentTests
{
  public struct ComponentTestData
  {
    internal GraphComponent<object, object> Component { get; init; }
    internal Graph<object, object> Graph { get; init; }
    internal Action RemoveComponent { get; init; }
  }

  private static IEnumerable<ComponentTestData> Components
  {
    get
    {
      var graph = new Graph<object, object>();
      var node = graph.AddNode(new { });
      var edge = graph.AddEdge(graph.AddNode(new { }), graph.AddNode(new { }), new { });
      yield return new ComponentTestData
      {
        Component = node,
        Graph = graph,
        RemoveComponent = () => graph.RemoveNode(node),
      };
      yield return new ComponentTestData
      {
        Component = edge,
        Graph = graph,
        RemoveComponent = () => graph.RemoveEdge(edge),
      };
    }
  }

  private static IEnumerable<GraphComponent<object, object>[]> ComponentPairsInSameGraph
  {
    get
    {
      var graph = new Graph<object, object>();
      var node1 = graph.AddNode(new { });
      var node2 = graph.AddNode(new { });
      var edge1 = graph.AddEdge(graph.AddNode(new { }), graph.AddNode(new { }), new { });
      var edge2 = graph.AddEdge(graph.AddNode(new { }), graph.AddNode(new { }), new { });
      yield return new GraphComponent<object, object>[] { node1, node2 };
      yield return new GraphComponent<object, object>[] { node1, edge1 };
      yield return new GraphComponent<object, object>[] { node1, edge2 };
      yield return new GraphComponent<object, object>[] { node2, edge1 };
      yield return new GraphComponent<object, object>[] { node2, edge2 };
      yield return new GraphComponent<object, object>[] { edge1, edge2 };
    }
  }

  private static IEnumerable<GraphComponent<object, object>[]> ComponentPairsInDifferentGraphs
  {
    get
    {
      var graph1 = new Graph<object, object>();
      var graph2 = new Graph<object, object>();
      var node1 = graph1.AddNode(new { });
      var edge1 = graph1.AddEdge(graph1.AddNode(new { }), graph1.AddNode(new { }), new { });
      var node2 = graph2.AddNode(new { });
      var edge2 = graph2.AddEdge(graph2.AddNode(new { }), graph2.AddNode(new { }), new { });
      yield return new GraphComponent<object, object>[] { node1, node2 };
      yield return new GraphComponent<object, object>[] { node1, edge2 };
      yield return new GraphComponent<object, object>[] { edge1, node2 };
      yield return new GraphComponent<object, object>[] { edge1, edge2 };
    }
  }

  [TestCaseSource(nameof(Components))]
  public void NewComponent_IsValid(ComponentTestData data)
  {
    // ASSERT
    Assert.That(data.Component.IsValid, Is.True);
  }

  [TestCaseSource(nameof(Components))]
  public void Invalidate_ValidComponent_IsInvalid(ComponentTestData data)
  {
    // ACT
    data.Component.Invalidate();

    // ASSERT
    Assert.That(data.Component.IsValid, Is.False);
  }

  [TestCaseSource(nameof(Components))]
  public void Invalidate_InvalidComponent_ThrowsInvalidOperationException(ComponentTestData data)
  {
    // ARRANGE
    data.Component.Invalidate();

    // ASSERT
    Assert.That(() => data.Component.Invalidate(), Throws.InvalidOperationException);
  }

  [TestCaseSource(nameof(Components))]
  public void RemoveComponent_ComponentIsInvalid(ComponentTestData data)
  {
    // ACT
    data.RemoveComponent();

    // ASSERT
    Assert.That(data.Component.IsValid, Is.False);
  }

  [TestCaseSource(nameof(Components))]
  public void IsIn_CorrectGraph_True(ComponentTestData data)
  {
    // ASSERT
    Assert.That(data.Component.IsIn(data.Graph), Is.True);
  }

  [TestCaseSource(nameof(Components))]
  public void IsIn_OtherGraph_False(ComponentTestData data)
  {
    // ARRANGE
    var otherGraph = new Graph<object, object>();

    // ASSERT
    Assert.That(data.Component.IsIn(otherGraph), Is.False);
  }

  [TestCaseSource(nameof(Components))]
  public void IsIn_InvalidComponent_ThrowsInvalidOperationException(ComponentTestData data)
  {
    // ARRANGE
    data.Component.Invalidate();

    // ASSERT
    Assert.That(() => data.Component.IsIn(data.Graph), Throws.InvalidOperationException);
  }

  [TestCaseSource(nameof(ComponentPairsInSameGraph))]
  public void IsInSameGraphAs_ComponentInSameGraph_True(GraphComponent<object, object> component,
    GraphComponent<object, object> otherComponent)
  {
    // ASSERT
    Assert.That(component.IsInSameGraphAs(otherComponent), Is.True);
  }

  [TestCaseSource(nameof(ComponentPairsInDifferentGraphs))]
  public void IsInSameGraphAs_ComponentInDifferentGraphs_False(
    GraphComponent<object, object> component, GraphComponent<object, object> otherComponent)
  {
    // ASSERT
    Assert.That(component.IsInSameGraphAs(otherComponent), Is.False);
  }
}