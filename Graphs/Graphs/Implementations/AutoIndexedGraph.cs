using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

public sealed class AutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  #region Fields

  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  private readonly IIndexProvider<TNodeData, TIndex> indexProvider;

  #endregion

  #region Constructors

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

  #endregion

  #region Data Access

  /// <inheritdoc />
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  /// <inheritdoc />
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  /// <inheritdoc />
  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  /// <inheritdoc />
  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  /// <inheritdoc />
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  /// <inheritdoc />
  public bool Contains(TIndex index) => graph.Contains(index);

  /// <inheritdoc />
  public int Order => graph.Order;

  /// <inheritdoc />
  public int Size => graph.Size;

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  /// <inheritdoc />
  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  #endregion

  #region Data Modification

  /// <inheritdoc />
  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data) =>
    graph.AddNode(indexProvider.GetIndex(data), data);

  /// <inheritdoc />
  public bool TryAddNode(TNodeData data, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryAddNode(indexProvider.GetIndex(data), data, out node);

  /// <inheritdoc />
  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data) =>
    graph.AddEdge(origin, destination, data);

  /// <inheritdoc />
  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge) =>
    graph.TryAddEdge(origin, destination, data, out edge);

  /// <inheritdoc />
  public bool RemoveNode(TIndex index) => graph.RemoveNode(index);

  /// <inheritdoc />
  public bool RemoveNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.RemoveNode(index, out node);

  /// <inheritdoc />
  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  /// <inheritdoc />
  bool IGraph<TNodeData, TEdgeData>.RemoveNode(INode<TNodeData, TEdgeData> node) =>
    ((IGraph<TNodeData, TEdgeData>)graph).RemoveNode(node);

  /// <inheritdoc />
  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  /// <inheritdoc />
  bool IGraph<TNodeData, TEdgeData>.RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    ((IGraph<TNodeData, TEdgeData>)graph).RemoveEdge(edge);

  /// <inheritdoc />
  public int RemoveNodesWhere(Predicate<TNodeData> predicate) => graph.RemoveNodesWhere(predicate);

  /// <inheritdoc />
  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate) => graph.RemoveEdgesWhere(predicate);

  /// <inheritdoc />
  public void Clear() => graph.Clear();

  #endregion

  #region Transformation

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
    TransformToIndexedGraph(nodeDataTransformation, edgeDataTransformation, indexTransformation,
      () => indexEqualityComparer);

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation,
      Func<IEqualityComparer<TIndexTransformed>?> indexEqualityComparerFactoryMethod
    ) where TIndexTransformed : notnull =>
    graph.Transform(nodeDataTransformation, edgeDataTransformation,
      indexTransformation, indexEqualityComparerFactoryMethod);

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph() => ToUnindexedGraph(data => data, data => data);

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) =>
    TransformToUnindexedGraph(cloneNodeData, cloneEdgeData);

  public Graph<TNodeDataTransformed, TEdgeDataTransformed> TransformToUnindexedGraph<TNodeDataTransformed,
    TEdgeDataTransformed>(
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation) =>
    graph.TransformToUnindexedGraph(nodeDataTransformation, edgeDataTransformation);

  #endregion
}