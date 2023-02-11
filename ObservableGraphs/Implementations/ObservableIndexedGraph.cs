using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs.Observable;

public sealed class ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IObservableIndexedGraph<TIndex, TNodeData, TEdgeData>,
  IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  #region Fields

  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  #endregion

  #region Constructors

  public ObservableIndexedGraph(IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(() => nodeIndexEqualityComparer) { }

  public ObservableIndexedGraph(Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    graph = new IndexedGraph<TIndex, TNodeData, TEdgeData>(nodeIndexEqualityComparerFactoryMethod);
  }

  internal ObservableIndexedGraph(IndexedGraph<TIndex, TNodeData, TEdgeData> graph)
  {
    this.graph = graph;
  }

  #endregion

  #region Events

  public event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>>? GraphChanged;

  #endregion

  #region Data Access

  #region Nodes

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;
  
  IReadOnlyCollection<IIndexedNode<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Nodes => Nodes;

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];
  
  IIndexedNode<TIndex, TNodeData, TEdgeData> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.this[TIndex index] => this[index];
  
  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  #endregion

  #region Edges

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;
  
  IReadOnlyCollection<IIndexedEdge<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Edges => Edges;

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

  #region Data Manipulation

  #region Add Nodes

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data)
  {
    var node = graph.AddNode(index, data);
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return node;
  }

  IIndexedNode<TIndex, TNodeData, TEdgeData> IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>.AddNode(TIndex index, TNodeData data) => 
    AddNode(index, data);

  public bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryAddNode(index, data, out node))
      return false;
    InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return true;
  }

  #endregion

  #region Remove Nodes

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => node.IsValid && RemoveNode(node.Index);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public bool RemoveNode(IIndexedNode<TIndex, TNodeData, TEdgeData> node) =>
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

  bool IIndexedGraph<TIndex, TNodeData, TEdgeData>.RemoveNode(TIndex index, [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
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

  IIndexedEdge<TIndex, TNodeData, TEdgeData> IIndexedGraph<TIndex, TNodeData, TEdgeData>.AddEdge(TIndex origin, TIndex destination, TEdgeData data) => 
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
    var removedEdge = graph.RemoveEdge(edge);
    if (removedEdge)
    {
      InvokeGraphChanged(new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>
        { RemovedEdges = new[] { edge } });
    }
    return removedEdge;
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

  #region Clear

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

  #endregion

  #endregion

  private void InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> eventArgs) =>
    GraphChanged?.Invoke(this, eventArgs);
}