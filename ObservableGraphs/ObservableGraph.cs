using System.Diagnostics.CodeAnalysis;
using DataForge.Graphs;

namespace DataForge.ObservableGraphs;

public sealed class ObservableGraph<TNodeData, TEdgeData> : IObservableUnindexedGraph<TNodeData, TEdgeData>
{
  private readonly Graph<TNodeData, TEdgeData> graph = new();

  /// <inheritdoc />
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Nodes => graph.Nodes;

  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  /// <inheritdoc />
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges => graph.Edges;

  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  /// <inheritdoc />
  public int Order => graph.Order;

  /// <inheritdoc />
  public int Size => graph.Size;

  /// <inheritdoc />
  public event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>>? GraphChanged;

  /// <inheritdoc />
  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  /// <inheritdoc />
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  /// <inheritdoc />
  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = graph.AddNode(data);
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return node;
  }

  /// <inheritdoc />
  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(params TNodeData[] data) => AddNodes(data.AsEnumerable());

  /// <inheritdoc />
  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data)
  {
    var nodes = graph.AddNodes(data).ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedNodes = nodes });
    return nodes;
  }

  /// <inheritdoc />
  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    if (!node.IsValid || node is not Node<TNodeData, TEdgeData> typedNode)
      return false;
    var adjacentEdges = graph.Edges.Where(edge =>
        edge.Origin == node ||
        edge.Destination == node)
      .ToArray();
    var result = graph.RemoveNode(node);
    if (result)
      InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData>
      {
        RemovedEdges = adjacentEdges,
        RemovedNodes = new[] { typedNode },
      });
    return result;
  }

  /// <inheritdoc />
  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination,
    TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return edge;
  }

  public bool TryAddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return true;
  }

  /// <inheritdoc />
  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var edges = graph.Edges.ToArray();
    var removedNodes = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(node => graph.RemoveNode(node))
      .ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData>
    {
      RemovedNodes = removedNodes,
      RemovedEdges = edges.Where(edge => !edge.IsValid).ToArray(),
    });
    return removedNodes.Length;
  }

  /// <inheritdoc />
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    if (!edge.IsValid || edge is not Edge<TNodeData, TEdgeData> typedEdge)
      return false;
    var result = graph.RemoveEdge(typedEdge);
    if (result)
      InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { RemovedEdges = new[] { typedEdge } });
    return result;
  }

  /// <inheritdoc />
  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    var removedEdges = graph.Edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(edge => graph.RemoveEdge(edge))
      .ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { RemovedEdges = removedEdges });
    return removedEdges.Length;
  }

  /// <inheritdoc />
  public void Clear()
  {
    var eventArgs = new GraphChangedEventArgs<TNodeData, TEdgeData>
    {
      RemovedNodes = graph.Nodes.ToArray(),
      RemovedEdges = graph.Edges.ToArray(),
    };
    graph.Clear();
    InvokeGraphChanged(eventArgs);
  }

  private void InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData> eventArgs) =>
    GraphChanged?.Invoke(this, eventArgs);
}