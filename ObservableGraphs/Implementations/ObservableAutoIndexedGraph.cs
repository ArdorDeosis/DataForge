using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs.Observable;

public sealed class ObservableAutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IObservableIndexedGraph<TIndex, TNodeData, TEdgeData>,
  IAutoIndexedGraph<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  #region Fields

  private readonly AutoIndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  #endregion

  #region Constructors

  public ObservableAutoIndexedGraph(Func<TNodeData, TIndex> indexGeneratorFunction,
    IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(indexGeneratorFunction, () => nodeIndexEqualityComparer) { }

  public ObservableAutoIndexedGraph(IIndexProvider<TNodeData, TIndex> indexProvider,
    IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(indexProvider, () => nodeIndexEqualityComparer) { }

  public ObservableAutoIndexedGraph(Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
    : this(new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      nodeIndexEqualityComparerFactoryMethod) { }

  public ObservableAutoIndexedGraph(IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    graph = new AutoIndexedGraph<TIndex, TNodeData, TEdgeData>(indexProvider, nodeIndexEqualityComparerFactoryMethod);
  }

  #endregion

  #region Events

  public event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>>? GraphChanged;

  #endregion

  #region Data Access

  #region Nodes

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  IReadOnlyCollection<IIndexedNode<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.
    Nodes => graph.Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  IIndexedNode<TIndex, TNodeData, TEdgeData> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.this[TIndex index] =>
    this[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  #endregion

  #region Edges

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  IReadOnlyCollection<IIndexedEdge<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.
    Edges => graph.Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  #endregion

  #region Indices

  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  public bool Contains(TIndex index) => graph.Contains(index);

  #endregion

  #region Graph Metrics

  public int Order => graph.Order;


  public int Size => graph.Size;

  #endregion

  #endregion

  #region Data Modification

  #region Add Nodes

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = graph.AddNode(data);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return node;
  }

  IIndexedNode<TIndex, TNodeData, TEdgeData> IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>.AddNode(TNodeData data) =>
    AddNode(data);

  public bool TryAddNode(TNodeData data, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryAddNode(data, out node))
      return false;
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return true;
  }

  bool IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>.TryAddNode(TNodeData data,
    [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (graph.TryAddNode(data, out var typedNode))
    {
      InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
        { AddedNodes = new[] { typedNode } });
      node = typedNode;
      return true;
    }

    node = null;
    return false;
  }

  #endregion

  #region Remove Nodes

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => node.IsValid && RemoveNode(node.Index);

  public bool RemoveNode(IIndexedNode<TIndex, TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public bool RemoveNode(TIndex index) => RemoveNode(index, out _);

  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryGetNode(index, out node))
      return false;
    var eventArgs = new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
    {
      RemovedEdges = node.Edges,
      RemovedNodes = new[] { node },
    };
    if (!graph.RemoveNode(index))
      return false;
    InvokeGraphChanged(eventArgs);
    return true;
  }

  bool IIndexedGraph<TIndex, TNodeData, TEdgeData>.RemoveNode(TIndex index,
    [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    node = RemoveNode(index, out var typedNode) ? typedNode : null;
    return node is not null;
  }

  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var nodesToRemove = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray();
    var edgesToRemove = nodesToRemove
      .SelectMany(node => node.Edges)
      .ToHashSet();
    foreach (var node in nodesToRemove)
      graph.RemoveNode(node.Index);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
    {
      RemovedNodes = nodesToRemove,
      RemovedEdges = edgesToRemove,
    });
    return nodesToRemove.Length;
  }

  #endregion

  #region Add Edges

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return edge;
  }

  IIndexedEdge<TIndex, TNodeData, TEdgeData> IIndexedGraph<TIndex, TNodeData, TEdgeData>.AddEdge(TIndex origin,
    TIndex destination, TEdgeData data) =>
    AddEdge(origin, destination, data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return true;
  }

  #endregion

  #region Remove Edges

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge)
  {
    var result = graph.RemoveEdge(edge);
    if (result)
      InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
        { RemovedEdges = new[] { edge } });
    return result;
  }

  public bool RemoveEdge(IIndexedEdge<TIndex, TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

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

  public void ClearEdges()
  {
    var eventArgs = new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { RemovedEdges = graph.Edges };
    graph.ClearEdges();
    InvokeGraphChanged(eventArgs);
  }

  #endregion

  #endregion

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