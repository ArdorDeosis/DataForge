using System.Diagnostics.CodeAnalysis;
using DataForge.Graphs;

namespace DataForge.ObservableGraphs;

public sealed class ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IObservableIndexedGraph<TIndex, TNodeData, TEdgeData>,
  IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  public ObservableIndexedGraph(IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(() => nodeIndexEqualityComparer) { }

  public ObservableIndexedGraph(Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    graph = new IndexedGraph<TIndex, TNodeData, TEdgeData>(nodeIndexEqualityComparerFactoryMethod);
  }

  /// <inheritdoc />
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  /// <inheritdoc />
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  /// <inheritdoc />
  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  /// <inheritdoc />
  public int Order => graph.Order;

  /// <inheritdoc />
  public int Size => graph.Size;

  /// <inheritdoc />
  public event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>>? GraphChanged;

  /// <inheritdoc />
  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  /// <inheritdoc />
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  /// <inheritdoc />
  public bool Contains(TIndex index) => graph.Contains(index);

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  /// <inheritdoc />
  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data)
  {
    var node = graph.AddNode(index, data);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return node;
  }

  /// <inheritdoc />
  public bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryAddNode(index, data, out node))
      return false;
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return true;
  }

  /// <inheritdoc />
  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  /// <inheritdoc />
  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => node.IsValid && RemoveNode(node.Index);

  /// <inheritdoc />
  public bool RemoveNode(TIndex index) => RemoveNode(index, out _);

  /// <inheritdoc />
  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryGetNode(index, out node))
      return false;
    var nodeCopy = node; // out parameter cannot be used in lambda expression
    var adjacentEdges = graph.Edges.Where(edge =>
        edge.Origin == nodeCopy ||
        edge.Destination == nodeCopy)
      .ToArray();
    var result = graph.RemoveNode(index);
    if (result)
      InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
      {
        RemovedEdges = adjacentEdges,
        RemovedNodes = new[] { node },
      });
    return result;
  }

  /// <inheritdoc />
  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var edges = graph.Edges.ToArray();
    var removedNodes = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(node => graph.RemoveNode(node.Index))
      .ToArray();
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
    {
      RemovedNodes = removedNodes,
      RemovedEdges = edges.Where(edge => !edge.IsValid).ToArray(),
    });
    return removedNodes.Length;
  }

  /// <inheritdoc />
  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return edge;
  }

  /// <inheritdoc />
  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return true;
  }

  /// <inheritdoc />
  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge)
  {
    var result = graph.RemoveEdge(edge);
    if (result)
      InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
        { RemovedEdges = new[] { edge } });
    return result;
  }

  /// <inheritdoc />
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

  /// <inheritdoc />
  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    var removedEdges = graph.Edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(edge => graph.RemoveEdge(edge))
      .ToArray();
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { RemovedEdges = removedEdges });
    return removedEdges.Length;
  }

  /// <inheritdoc />
  public void Clear()
  {
    var eventArgs = new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
    {
      RemovedNodes = graph.Nodes.ToArray(),
      RemovedEdges = graph.Edges.ToArray(),
    };
    graph.Clear();
    InvokeGraphChanged(eventArgs);
  }

  private void InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> eventArgs) =>
    GraphChanged?.Invoke(this, eventArgs);
}