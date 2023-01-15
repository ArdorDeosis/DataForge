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

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  public bool Contains(TIndex index) => graph.Contains(index);
  
  public int Order => graph.Order;
  public int Size => graph.Size;
  
  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) => 
    graph.TryGetNode(index, out node);
  
  #endregion

  #region Data Modification

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data) => 
    graph.AddNode(indexProvider.GetIndex(data), data);

  public bool TryAddNode(TNodeData data, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) => 
    graph.TryAddNode(indexProvider.GetIndex(data), data, out node);

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data) =>
    graph.AddEdge(origin, destination, data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge) =>
    graph.TryAddEdge(origin, destination, data, out edge);

  public bool RemoveNode(TIndex index) => graph.RemoveNode(index);

  public bool RemoveNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) => graph.RemoveNode(index, out node);

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  bool IGraph<TNodeData, TEdgeData>.RemoveNode(INode<TNodeData, TEdgeData> node) => 
    ((IGraph<TNodeData, TEdgeData>)graph).RemoveNode(node);

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  bool IGraph<TNodeData, TEdgeData>.RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    ((IGraph<TNodeData, TEdgeData>)graph).RemoveEdge(edge);

  public int RemoveNodeWhere(Predicate<TNodeData> predicate) => graph.RemoveNodeWhere(predicate);

  public int RemoveEdgeWhere(Predicate<TEdgeData> predicate) => graph.RemoveEdgeWhere(predicate);

  public void Clear() => graph.Clear();

  #endregion
}