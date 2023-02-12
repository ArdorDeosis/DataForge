using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

public sealed class AutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  private readonly IIndexProvider<TNodeData, TIndex> indexProvider;

  public AutoIndexedGraph(Func<TNodeData, TIndex> indexGeneratorFunction,
    IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(indexGeneratorFunction, () => nodeIndexEqualityComparer) { }

  public AutoIndexedGraph(IIndexProvider<TNodeData, TIndex> indexProvider,
    IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(indexProvider, () => nodeIndexEqualityComparer) { }

  public AutoIndexedGraph(Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
    : this(new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      nodeIndexEqualityComparerFactoryMethod) { }

  public AutoIndexedGraph(IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    this.indexProvider = indexProvider;
    graph = new IndexedGraph<TIndex, TNodeData, TEdgeData>(nodeIndexEqualityComparerFactoryMethod);
  }

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  IReadOnlyCollection<IIndexedNode<TIndex, TNodeData, TEdgeData>>
    IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  IIndexedNode<TIndex, TNodeData, TEdgeData>
    IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.this[TIndex index] => graph[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  public bool TryGetNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  IReadOnlyCollection<IIndexedEdge<TIndex, TNodeData, TEdgeData>>
    IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Edges => graph.Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  public bool Contains(TIndex index) => graph.Contains(index);

  public int Order => graph.Order;

  public int Size => graph.Size;

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var index = indexProvider.GetCurrentIndex(data);
    var node = graph.AddNode(index, data);
    indexProvider.Move();
    return node;
  }

  IIndexedNode<TIndex, TNodeData, TEdgeData> IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>.AddNode(TNodeData data) =>
    AddNode(data);

  public bool TryAddNode(TNodeData data, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    var nodeAdded = graph.TryAddNode(indexProvider.GetCurrentIndex(data), data, out node);
    if (nodeAdded)
      indexProvider.Move();
    return nodeAdded;
  }

  bool IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>.TryAddNode(TNodeData data,
    [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    node = TryAddNode(data, out var indexedNode) ? indexedNode : null;
    return node is not null;
  }

  public bool RemoveNode(TIndex index) => graph.RemoveNode(index, out _);

  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.RemoveNode(index, out node);

  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    node = graph.RemoveNode(index, out var indexedNode) ? indexedNode : null;
    return node is not null;
  }

  public bool RemoveNode(IIndexedNode<TIndex, TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  public int RemoveNodesWhere(Predicate<TNodeData> predicate) => graph.RemoveNodesWhere(predicate);

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data) =>
    graph.AddEdge(origin, destination, data);

  IIndexedEdge<TIndex, TNodeData, TEdgeData> IIndexedGraph<TIndex, TNodeData, TEdgeData>.AddEdge(TIndex origin,
    TIndex destination, TEdgeData data) =>
    AddEdge(origin, destination, data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge) =>
    graph.TryAddEdge(origin, destination, data, out edge);

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  public bool RemoveEdge(IIndexedEdge<TIndex, TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  bool IGraph<TNodeData, TEdgeData>.RemoveEdge(IEdge<TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate) => graph.RemoveEdgesWhere(predicate);

  public void ClearEdges() => graph.ClearEdges();

  public void Clear() => graph.Clear();

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph() => graph.Clone();

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) =>
    graph.Clone(cloneNodeData, cloneEdgeData);

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation,
      IEqualityComparer<TIndexTransformed>? indexEqualityComparer = null
    ) where TIndexTransformed : notnull =>
    TransformToIndexedGraph(
      nodeDataTransformation,
      edgeDataTransformation,
      indexTransformation,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation,
      Func<IEqualityComparer<TIndexTransformed>?> indexEqualityComparerFactoryMethod
    ) where TIndexTransformed : notnull =>
    graph.Transform(
      nodeDataTransformation,
      edgeDataTransformation,
      indexTransformation,
      indexEqualityComparerFactoryMethod
    );

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph() =>
    ToUnindexedGraph(data => data, data => data);

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) =>
    TransformToUnindexedGraph(cloneNodeData, cloneEdgeData);

  public Graph<TNodeDataTransformed, TEdgeDataTransformed> TransformToUnindexedGraph<TNodeDataTransformed,
    TEdgeDataTransformed>(
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation) =>
    graph.TransformToUnindexedGraph(nodeDataTransformation, edgeDataTransformation);
  /// 
}