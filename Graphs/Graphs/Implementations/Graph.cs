using System.Diagnostics.CodeAnalysis;
using DataForge.Utilities;

namespace DataForge.Graphs;

public sealed class Graph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  #region Fields

  internal readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    IncomingEdges = new();

  internal readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    OutgoingEdges = new();

  private readonly HashSet<Node<TNodeData, TEdgeData>> nodes = new();

  private readonly HashSet<Edge<TNodeData, TEdgeData>> edges = new();

  #endregion

  #region Constructors

  public Graph()
  {
    Nodes = nodes.InReadOnlyWrapper();
    Edges = edges.InReadOnlyWrapper();
  }

  #endregion

  #region Data Access

  #region Nodes

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Nodes { get; }

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  public bool Contains(INode<TNodeData, TEdgeData> node) => nodes.Contains(node);

  #endregion

  #region Edges

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges { get; }

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Edges => Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  #endregion

  #region Graph Metrics

  public int Order => nodes.Count;

  public int Size => edges.Count;

  #endregion

  #endregion

  #region Data Modification

  #region Add Nodes

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = new Node<TNodeData, TEdgeData>(this, data);
    nodes.Add(node);
    return node;
  }

  INode<TNodeData, TEdgeData> IUnindexedGraph<TNodeData, TEdgeData>.AddNode(TNodeData data) => AddNode(data);


  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data) =>
    data.Select(AddNode).ToArray();

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(params TNodeData[] data) =>
    data.Select(AddNode).ToArray();

  #endregion

  #region Remove Nodes

  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    if (node is not Node<TNodeData, TEdgeData> castNode || !nodes.Remove(castNode))
      return false;
    foreach (var edge in OutgoingEdges[castNode].Concat(IncomingEdges[castNode]).ToArray())
      RemoveEdge(edge);
    OutgoingEdges.Clear(castNode);
    IncomingEdges.Clear(castNode);
    castNode.Invalidate();
    return true;
  }

  public int RemoveNodesWhere(Predicate<TNodeData> predicate) =>
    nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(RemoveNode)
      .Count();

  #endregion

  #region Add Edges

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination,
    TEdgeData data)
  {
    if (!nodes.Contains(origin))
      throw new ArgumentException("The origin node is not part of this graph.");
    if (!nodes.Contains(destination))
      throw new ArgumentException("The destination node is not part of this graph.");
    var edge = new Edge<TNodeData, TEdgeData>(this, origin, destination, data);
    edges.Add(edge);
    OutgoingEdges.Add(origin, edge);
    IncomingEdges.Add(destination, edge);
    return edge;
  }

  IEdge<TNodeData, TEdgeData> IUnindexedGraph<TNodeData, TEdgeData>.AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data)
  {
    if (origin is not Node<TNodeData, TEdgeData> typedOrigin)
      throw new ArgumentException("The origin node is not part of this graph.");
    if (destination is not Node<TNodeData, TEdgeData> typedDestination)
      throw new ArgumentException("The destination node is not part of this graph.");
    return AddEdge(typedOrigin, typedDestination, data);
  }

  public bool TryAddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge)
  {
    edge = nodes.Contains(origin) && nodes.Contains(destination) ? AddEdge(origin, destination, data) : null;
    return edge is not null;
  }

  #endregion

  #region Remove Edges

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    if (edge is not Edge<TNodeData, TEdgeData> castEdge || !edges.Remove(castEdge))
      return false;
    OutgoingEdges.Remove(castEdge.Origin, castEdge);
    IncomingEdges.Remove(castEdge.Destination, castEdge);
    castEdge.Invalidate();
    return true;
  }

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate) =>
    edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(RemoveEdge)
      .Count();

  public void ClearEdges()
  {
    foreach (var edge in edges)
      edge.Invalidate();
    edges.Clear();
    IncomingEdges.Clear();
    OutgoingEdges.Clear();
  }

  #endregion

  #region Clear

  public void Clear()
  {
    foreach (var node in nodes)
      node.Invalidate();
    foreach (var edge in edges)
      edge.Invalidate();
    nodes.Clear();
    edges.Clear();
    IncomingEdges.Clear();
    OutgoingEdges.Clear();
  }

  #endregion

  #endregion

  #region Clone & Transformation

  public Graph<TNodeData, TEdgeData> Clone() => Transform(data => data, data => data);

  public Graph<TNodeData, TEdgeData> Clone(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) =>
    Transform(cloneNodeData, cloneEdgeData);

  public Graph<TNodeDataTransformed, TEdgeDataTransformed>
    Transform<TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation
    )
  {
    var graph = new Graph<TNodeDataTransformed, TEdgeDataTransformed>();
    var nodeMap = nodes.ToDictionary(node => node, node => graph.AddNode(nodeDataTransformation(node.Data)));
    foreach (var edge in edges)
      graph.AddEdge(nodeMap[edge.Origin], nodeMap[edge.Destination], edgeDataTransformation(edge.Data));
    return graph;
  }

  #endregion

  #region To Indexed Graph

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction, IEqualityComparer<TIndex>? indexEqualityComparer = null)
    where TIndex : notnull =>
    ToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      data => data,
      data => data,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider, IEqualityComparer<TIndex>? indexEqualityComparer = null)
    where TIndex : notnull =>
    ToIndexedGraph(
      indexProvider,
      data => data,
      data => data,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    ToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      data => data,
      data => data,
      indexEqualityComparerFactoryMethod
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    ToIndexedGraph(
      indexProvider,
      data => data,
      data => data,
      indexEqualityComparerFactoryMethod
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      cloneNodeData,
      cloneEdgeData,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(
      indexProvider,
      cloneNodeData,
      cloneEdgeData,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    TransformToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      cloneNodeData,
      cloneEdgeData,
      indexEqualityComparerFactoryMethod
    );

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    TransformToIndexedGraph(
      indexProvider,
      cloneNodeData,
      cloneEdgeData,
      indexEqualityComparerFactoryMethod
    );

  public IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed> TransformToIndexedGraph<TIndex,
    TNodeDataTransformed, TEdgeDataTransformed>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      nodeDataTransformation,
      edgeDataTransformation,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed> TransformToIndexedGraph<TIndex,
    TNodeDataTransformed, TEdgeDataTransformed>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(
      indexProvider,
      nodeDataTransformation,
      edgeDataTransformation,
      () => indexEqualityComparer
    );

  public IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed> TransformToIndexedGraph<TIndex,
    TNodeDataTransformed, TEdgeDataTransformed>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    TransformToIndexedGraph(
      new StatelessIndexProvider<TNodeData, TIndex>(indexGeneratorFunction),
      nodeDataTransformation,
      edgeDataTransformation,
      indexEqualityComparerFactoryMethod
    );

  public IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed> TransformToIndexedGraph<TIndex,
    TNodeDataTransformed, TEdgeDataTransformed>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull
  {
    var indexedGraph =
      new IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>(indexEqualityComparerFactoryMethod);
    var nodeMap = nodes.ToDictionary(
      node => node,
      node =>
      {
        var addedNode = indexedGraph
          .AddNode(indexProvider.GetCurrentIndex(node.Data), nodeDataTransformation(node.Data)).Index;
        indexProvider.Move();
        return addedNode;
      });
    foreach (var edge in edges)
      indexedGraph.AddEdge(nodeMap[edge.Origin], nodeMap[edge.Destination], edgeDataTransformation(edge.Data));
    return indexedGraph;
  }
  
  #endregion
}