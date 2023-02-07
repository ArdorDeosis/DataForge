using DataForge.Graphs;
using NUnit.Framework;

namespace DataForge.ObservableGraphs.Tests.UnindexedGraph;

[TestFixture]
public class GraphChangedEventTests
{
#pragma warning disable CS8618 // field not initialized warning; is initialized in setup method
  private static ObservableGraph<int, int> graph;
#pragma warning restore CS8618
  private static readonly List<GraphChangedEventArgs<int, int>> EventList = new();

  [SetUp]
  public void Setup()
  {
    graph = new ObservableGraph<int, int>();
    EventList.Clear();
  }

  private static void LogEvent(object? sender, GraphChangedEventArgs<int, int> eventArgs) =>
    EventList.Add(eventArgs);

  public delegate Node<int, int> AddNode(ObservableGraph<int, int> graph, int data);

  public delegate void RemoveNode(ObservableGraph<int, int> graph, Node<int, int> node);

  public delegate Edge<int, int> AddEdge(ObservableGraph<int, int> graph, Node<int, int> origin,
    Node<int, int> destination, int data);

  public static IEnumerable<AddEdge> AddEdgeActions()
  {
    // ReSharper disable ParameterHidesMember
    yield return (graph, origin, destination, data) =>
    {
      try
      {
        return graph.AddEdge(origin, destination, data);
      }
      catch
      {
        return null!;
      }
    };
    yield return (graph, origin, destination, data) =>
      graph.TryAddEdge(origin, destination, data, out var edge) ? edge : null!;
    // ReSharper restore ParameterHidesMember
  }

  [Test]
  public void AddNode_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    graph.GraphChanged += LogEvent;

    // ACT
    var node = graph.AddNode(default);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.EquivalentTo(new[] { node }));
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [Test]
  public void AddNodes_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    graph.GraphChanged += LogEvent;

    // ACT
    var nodes = graph.AddNodes(0xC0FFEE, 0xBEEF, 0xF00D);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.EquivalentTo(nodes));
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [TestCaseSource(nameof(AddEdgeActions))]
  public void AddEdge_EdgeIsAdded_EventIsFiredOnceWithCorrectData(AddEdge addEdge)
  {
    // ARRANGE
    var origin = graph.AddNode(default);
    var destination = graph.AddNode(default);
    graph.GraphChanged += LogEvent;

    // ACT
    var edge = addEdge(graph, origin, destination, default);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.EquivalentTo(new[] { edge }));
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [TestCaseSource(nameof(AddEdgeActions))]
  public void AddEdge_EdgeIsNotAdded_EventIsNotFired(AddEdge addEdge)
  {
    // ARRANGE
    graph.GraphChanged += LogEvent;

    // ACT
    addEdge(graph, default!, default!, default);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [Test]
  public void RemoveNode_NodeIsRemoved_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var node = graph.AddNode(default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNode(node);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(new[] { node }));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [Test]
  public void RemoveNode_NodeAndAdjacentEdgesAreRemoved_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var origin = graph.AddNode(default);
    var destination = graph.AddNode(default);
    var edge1 = graph.AddEdge(origin, origin, default);
    var edge2 = graph.AddEdge(origin, destination, default);
    var edge3 = graph.AddEdge(destination, origin, default);
    graph.AddEdge(destination, destination, default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNode(origin);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(new[] { origin }));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.EquivalentTo(new[] { edge1, edge2, edge3 }));
  }

  [Test]
  public void RemoveNode_NoNodeIsRemoved_EventIsNotFired()
  {
    // ARRANGE
    var removedNode = graph.AddNode(default);
    graph.RemoveNode(removedNode);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNode(removedNode);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [Test]
  public void RemoveEdge_EdgeIsRemoved_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var edge = graph.AddEdge(
      graph.AddNode(default),
      graph.AddNode(default),
      default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.EquivalentTo(new[] { edge }));
  }

  [Test]
  public void RemoveEdge_NoEdgeIsRemoved_EventIsNotFired()
  {
    // ARRANGE
    var edge = graph.AddEdge(
      graph.AddNode(default),
      graph.AddNode(default),
      default);
    graph.RemoveEdge(edge);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveEdge(edge);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [Test]
  public void Clear_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var node1 = graph.AddNode(default);
    var node2 = graph.AddNode(default);
    var edge1 = graph.AddEdge(node1, node2, default);
    var edge2 = graph.AddEdge(node2, node1, default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.Clear();

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(new[] { node1, node2 }));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.EquivalentTo(new[] { edge1, edge2 }));
  }

  [Test]
  public void RemoveNodesWhere_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    bool Predicate(int data) => data > 0;
    var nodes = graph.AddNodes(0xC0FFEE, 0xBEEF, 0, int.MinValue);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(nodes.Where(node => Predicate(node.Data))));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [Test]
  public void RemoveNodesWhere_AdjacentEdgesAreRemoved_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    bool Predicate(int data) => data > 0;
    var node0 = graph.AddNode(0);
    var node1 = graph.AddNode(1);
    var edgesToRemove = new[]
    {
      graph.AddEdge(node0, node1, default),
      graph.AddEdge(node1, node0, default),
      graph.AddEdge(node1, node1, default),
    };
    graph.AddEdge(node0, node0, default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(new []{node1}));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.EquivalentTo(edgesToRemove));
  }

  [Test]
  public void RemoveEdgesWhere_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    bool Predicate(int data) => data > 0;
    var data = new[] { 0xC0FFEE, 0xBEEF, 0, int.MinValue };
    var edges = data.Select(value => graph.AddEdge(
      graph.AddNode(default),
      graph.AddNode(default),
      value)).ToArray();
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveEdgesWhere(Predicate);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.EquivalentTo(edges.Where(edge => Predicate(edge.Data))));
  }
}