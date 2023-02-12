using NUnit.Framework;

namespace DataForge.Graphs.Observable.Tests.AutoIndexedGraph;

[TestFixture]
public class GraphChangedEventTests
{
  [SetUp]
  public void Setup()
  {
    graph = new ObservableAutoIndexedGraph<int, int, int>(data => data);
    EventList.Clear();
  }
#pragma warning disable CS8618 // field not initialized warning; is initialized in setup method
  private static ObservableAutoIndexedGraph<int, int, int> graph;
  private static readonly List<IndexedGraphChangedEventArgs<int, int, int>> EventList = new();

  private static void LogEvent(object? sender, IndexedGraphChangedEventArgs<int, int, int> eventArgs)
  {
    EventList.Add(eventArgs);
  }

  public delegate IndexedNode<int, int, int> AddNode(ObservableAutoIndexedGraph<int, int, int> graph, int index);

  public delegate void RemoveNode(ObservableAutoIndexedGraph<int, int, int> graph, IndexedNode<int, int, int> node);

  public delegate IndexedEdge<int, int, int> AddEdge(ObservableAutoIndexedGraph<int, int, int> graph, int origin,
    int destination, int data);

  public static IEnumerable<AddNode> AddNodeActions()
  {
    // ReSharper disable ParameterHidesMember
    yield return (graph, index) =>
    {
      try
      {
        return graph.AddNode(index);
      }
      catch
      {
        return null!;
      }
    };
    yield return (graph, index) => graph.TryAddNode(index, out var node) ? node : null!;
    // ReSharper restore ParameterHidesMember
  }

  public static IEnumerable<RemoveNode> RemoveNodeActions()
  {
    // ReSharper disable ParameterHidesMember
    yield return (graph, node) => graph.RemoveNode(node);
    yield return (graph, node) => graph.RemoveNode(node.Index);
    yield return (graph, node) => graph.RemoveNode(node.Index, out _);
    // ReSharper restore ParameterHidesMember
  }

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

  [TestCaseSource(nameof(AddNodeActions))]
  public void AddNode_NodeIsAdded_EventIsFiredOnceWithCorrectData(AddNode addNode)
  {
    // ARRANGE
    graph.GraphChanged += LogEvent;

    // ACT
    var node = addNode(graph, default);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.EquivalentTo(new[] { node }));
    Assert.That(eventArgs.RemovedNodes, Is.Empty);
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [TestCaseSource(nameof(AddNodeActions))]
  public void AddNode_NodeIsNotAdded_EventIsNotFired(AddNode addNode)
  {
    // ARRANGE
    const int index = 0xBEEF;
    graph.AddNode(index);
    graph.GraphChanged += LogEvent;

    // ACT
    addNode(graph, index);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [TestCaseSource(nameof(AddEdgeActions))]
  public void AddEdge_EdgeIsAdded_EventIsFiredOnceWithCorrectData(AddEdge addEdge)
  {
    // ARRANGE
    const int index1 = 0xBEEF;
    const int index2 = 0xF00D;
    const int data = 0xC0FFEE;
    graph.AddNode(index1);
    graph.AddNode(index2);
    graph.GraphChanged += LogEvent;

    // ACT
    var edge = addEdge(graph, index1, index2, data);

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
    addEdge(graph, default, default, default);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [TestCaseSource(nameof(RemoveNodeActions))]
  public void RemoveNode_NodeIsRemoved_EventIsFiredOnceWithCorrectData(RemoveNode removeNode)
  {
    // ARRANGE
    var node = graph.AddNode(default);
    graph.GraphChanged += LogEvent;

    // ACT
    removeNode(graph, node);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(new[] { node }));
    Assert.That(eventArgs.AddedEdges, Is.Empty);
    Assert.That(eventArgs.RemovedEdges, Is.Empty);
  }

  [TestCaseSource(nameof(RemoveNodeActions))]
  public void RemoveNode_NodeAndAdjacentEdgesAreRemoved_EventIsFiredOnceWithCorrectData(RemoveNode removeNode)
  {
    // ARRANGE
    var origin = graph.AddNode(0xBEEF);
    var destination = graph.AddNode(0xC0FFEE);
    var edge1 = graph.AddEdge(origin.Index, origin.Index, default);
    var edge2 = graph.AddEdge(origin.Index, destination.Index, default);
    var edge3 = graph.AddEdge(destination.Index, origin.Index, default);
    graph.AddEdge(destination.Index, destination.Index, default);
    graph.GraphChanged += LogEvent;

    // ACT
    removeNode(graph, origin);

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
    const int index = 0xBEEF;
    var removedNode = graph.AddNode(index);
    graph.RemoveNode(removedNode);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNode(removedNode);
    graph.RemoveNode(index);
    graph.RemoveNode(index, out _);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [Test]
  public void RemoveEdge_EdgeIsRemoved_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var edge = graph.AddEdge(
      graph.AddNode(0xC0FFEE).Index,
      graph.AddNode(0xBEEF).Index,
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
      graph.AddNode(0xC0FFEE).Index,
      graph.AddNode(0xBEEF).Index,
      default);
    graph.RemoveEdge(edge);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveEdge(edge);
    (graph as IGraph<int, int>).RemoveEdge(edge);

    // ASSERT
    Assert.That(EventList, Is.Empty);
  }

  [Test]
  public void Clear_EventIsFiredOnceWithCorrectData()
  {
    // ARRANGE
    var node1 = graph.AddNode(0xC0FFEE);
    var node2 = graph.AddNode(0xBEEF);
    var edge1 = graph.AddEdge(node1.Index, node2.Index, default);
    var edge2 = graph.AddEdge(node2.Index, node1.Index, default);
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

    var nodes = new[]
    {
      graph.AddNode(0xC0FFEE),
      graph.AddNode(0xBEEF),
      graph.AddNode(0),
      graph.AddNode(int.MinValue),
    };
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

    var indices = new[] { 0, 1 };
    var nodes = indices.Select(index => graph.AddNode(index)).ToArray();
    var edgesToRemove = new[]
    {
      graph.AddEdge(0, 1, default),
      graph.AddEdge(1, 0, default),
      graph.AddEdge(1, 1, default),
    };
    graph.AddEdge(0, 0, default);
    graph.GraphChanged += LogEvent;

    // ACT
    graph.RemoveNodesWhere(Predicate);

    // ASSERT
    Assert.That(EventList, Has.Count.EqualTo(1));
    var eventArgs = EventList[0];
    Assert.That(eventArgs.AddedNodes, Is.Empty);
    Assert.That(eventArgs.RemovedNodes, Is.EquivalentTo(nodes.Where(node => Predicate(node.Data))));
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
      graph.AddNode(value).Index,
      graph.AddNode(value + 1).Index,
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