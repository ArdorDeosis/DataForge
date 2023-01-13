using System.Diagnostics.CodeAnalysis;

namespace Graph;

public sealed class AutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IAutoIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;

  private readonly IIndexProvider<TNodeData, TIndex> indexProvider;

  // ### CONSTRUCTORS ###

  // TODO: constructors

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

  public IEnumerable<TIndex> NodeIndices => graph.NodeIndices;

  // TODO: should these be read-only collections?
  public IEnumerable<IIndexedNode<TIndex, TNodeData, TEdgeData>> Nodes => graph.Nodes;
  IEnumerable<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public IEnumerable<TIndex> Indices { get; }

  public IIndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => throw new NotImplementedException();

  public bool Contains(INode<TNodeData, TEdgeData> node)
  {
    graph.Contains(node);
  }

  public bool Contains(IEdge<TNodeData, TEdgeData> edge)
  {
    throw new NotImplementedException();
  }

  IEnumerable<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  public IEnumerable<IIndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => graph.Edges;

  public int Order => graph.Order;
  public int Size => graph.Size;

  // TODO: should these have custom exceptions?
  public IIndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  public IIndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);
  public bool TryGetNode(TIndex index, out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    throw new NotImplementedException();
  }

  public bool TryGetNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  public bool Contains(TIndex index) => graph.Contains(index);

  // TODO: copy constructors

  // ### ADDITION & REMOVAL ###

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data) => 
    graph.AddNode(indexProvider.GetIndex(data), data);

  public bool TryAddNode(TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryAddNode(indexProvider.GetIndex(data), data, out node);

  public bool RemoveNode(TIndex index) => graph.RemoveNode(index);
  public bool RemoveNode(TIndex index, out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    throw new NotImplementedException();
  }

  public bool RemoveNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.RemoveNode(index, out node);

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination,
    TEdgeData data) =>
    graph.AddEdge(origin, destination, data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge) =>
    graph.TryAddEdge(origin, destination, data, out edge);

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(IndexedNode<TIndex, TNodeData, TEdgeData> origin,
    IndexedNode<TIndex, TNodeData, TEdgeData> destination, TEdgeData data) =>
    graph.AddEdge(origin, destination, data);

  public bool TryAddEdge(IndexedNode<TIndex, TNodeData, TEdgeData> origin,
    IndexedNode<TIndex, TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge) =>
    graph.TryAddEdge(origin, destination, data, out edge);

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    throw new NotImplementedException();
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    throw new NotImplementedException();
  }

  public void Clear() => graph.Clear();
}