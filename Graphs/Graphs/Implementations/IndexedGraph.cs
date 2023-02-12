using System.Diagnostics.CodeAnalysis;
using DataForge.Utilities;

namespace DataForge.Graphs;

public sealed class IndexedGraph<TIndex, TNodeData, TEdgeData> :
  IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  internal readonly MultiValueDictionary<TIndex, IndexedEdge<TIndex, TNodeData, TEdgeData>>
    IncomingEdges = new();

  internal readonly MultiValueDictionary<TIndex, IndexedEdge<TIndex, TNodeData, TEdgeData>>
    OutgoingEdges = new();

  private readonly Dictionary<TIndex, IndexedNode<TIndex, TNodeData, TEdgeData>> nodes;

  private readonly HashSet<IndexedEdge<TIndex, TNodeData, TEdgeData>> edges;

  private readonly Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod;

  public IndexedGraph(IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(() => nodeIndexEqualityComparer) { }

  public IndexedGraph(Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    this.nodeIndexEqualityComparerFactoryMethod = nodeIndexEqualityComparerFactoryMethod;
    nodes = new Dictionary<TIndex, IndexedNode<TIndex, TNodeData, TEdgeData>>(
      nodeIndexEqualityComparerFactoryMethod());
    edges = new HashSet<IndexedEdge<TIndex, TNodeData, TEdgeData>>();

    Nodes = nodes.Values.InReadOnlyWrapper();
    Edges = edges.InReadOnlyWrapper();
    Indices = nodes.Keys.InReadOnlyWrapper();
  }

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes { get; }

  IReadOnlyCollection<IIndexedNode<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.
    Nodes => Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  // TODO: should these have custom exceptions?

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => nodes[index];

  IIndexedNode<TIndex, TNodeData, TEdgeData> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.this[TIndex index] =>
    this[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => nodes[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) =>
    nodes.TryGetValue(index, out var node) ? node : null;

  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    nodes.TryGetValue(index, out node);

  public bool Contains(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && nodes.ContainsValue(indexedNode);

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges { get; }

  IReadOnlyCollection<IIndexedEdge<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.
    Edges => Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  public IReadOnlyCollection<TIndex> Indices { get; }

  public bool Contains(TIndex index) => nodes.ContainsKey(index);

  public int Order => nodes.Count;

  public int Size => edges.Count;

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data)
  {
    if (nodes.ContainsKey(index))
      throw new InvalidOperationException($"Node with index {index} already exists.");
    var node = new IndexedNode<TIndex, TNodeData, TEdgeData>(this, index, data);
    nodes.Add(index, node);
    return node;
  }

  IIndexedNode<TIndex, TNodeData, TEdgeData> IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>.AddNode(TIndex index,
    TNodeData data) =>
    AddNode(index, data);

  public bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    node = nodes.ContainsKey(index) ? null : AddNode(index, data);
    return node is not null;
  }

  public bool RemoveNode(TIndex index) => RemoveNode(index, out _);

  public bool RemoveNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!nodes.Remove(index, out var retrievedNode))
    {
      node = null;
      return false;
    }

    node = retrievedNode;

    foreach (var edge in node.Edges)
    {
      edges.Remove(edge);
      edge.Invalidate();
    }

    retrievedNode.Invalidate();
    return true;
  }

  bool IIndexedGraph<TIndex, TNodeData, TEdgeData>.RemoveNode(TIndex index,
    [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    node = RemoveNode(index, out var retrievedNode) ? retrievedNode : null;
    return node is not null;
  }

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) =>
    node.Graph == this && node.IsValid && RemoveNode(node.Index);

  public bool RemoveNode(IIndexedNode<TIndex, TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    return nodes.Values
      .Where(node => predicate(node.Data))
      .Select(node => node.Index)
      .ToArray()
      .Where(RemoveNode)
      .Count();
  }

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination,
    TEdgeData data)
  {
    if (!nodes.ContainsKey(origin))
      throw new ArgumentException("The origin node does not exist in the graph.");
    if (!nodes.ContainsKey(destination))
      throw new ArgumentException("The destination node does not exist in the graph.");
    var edge = new IndexedEdge<TIndex, TNodeData, TEdgeData>(this, origin, destination, data);
    edges.Add(edge);
    OutgoingEdges.Add(origin, edge);
    IncomingEdges.Add(destination, edge);
    return edge;
  }

  IIndexedEdge<TIndex, TNodeData, TEdgeData> IIndexedGraph<TIndex, TNodeData, TEdgeData>.AddEdge(TIndex origin,
    TIndex destination, TEdgeData data) =>
    AddEdge(origin, destination, data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge)
  {
    edge = Contains(origin) && Contains(destination) ? AddEdge(origin, destination, data) : null;
    return edge is not null;
  }

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge)
  {
    if (!edges.Remove(edge)) return false;
    IncomingEdges.Remove(edge.DestinationIndex, edge);
    OutgoingEdges.Remove(edge.OriginIndex, edge);
    edge.Invalidate();
    return true;
  }

  public bool RemoveEdge(IIndexedEdge<TIndex, TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    return edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(RemoveEdge)
      .Count();
  }

  public void ClearEdges()
  {
    foreach (var edge in edges)
      edge.Invalidate();
    edges.Clear();
    IncomingEdges.Clear();
    OutgoingEdges.Clear();
  }

  public void Clear()
  {
    foreach (var node in nodes.Values)
      node.Invalidate();
    foreach (var edge in edges)
      edge.Invalidate();
    nodes.Clear();
    edges.Clear();
    IncomingEdges.Clear();
    OutgoingEdges.Clear();
  }

  public IndexedGraph<TIndex, TNodeData, TEdgeData> Clone()
  {
    return Transform(data => data, data => data, index => index, nodeIndexEqualityComparerFactoryMethod);
  }

  public IndexedGraph<TIndex, TNodeData, TEdgeData> Clone(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData)
  {
    return Transform(cloneNodeData, cloneEdgeData, index => index, nodeIndexEqualityComparerFactoryMethod);
  }

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    Transform<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation,
      IEqualityComparer<TIndexTransformed>? indexEqualityComparer = null
    ) where TIndexTransformed : notnull
  {
    return Transform(nodeDataTransformation, edgeDataTransformation, indexTransformation, () => indexEqualityComparer);
  }

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    Transform<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation,
      Func<IEqualityComparer<TIndexTransformed>?> indexEqualityComparerFactoryMethod
    ) where TIndexTransformed : notnull
  {
    var transformedGraph = new IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      indexEqualityComparerFactoryMethod);
    var indexMap = nodes.Keys.ToDictionary(index => index, indexTransformation);
    foreach (var indexPair in indexMap)
      transformedGraph.AddNode(indexPair.Value, nodeDataTransformation(nodes[indexPair.Key].Data));
    foreach (var edge in edges)
      transformedGraph.AddEdge(indexMap[edge.Origin.Index], indexMap[edge.Destination.Index],
        edgeDataTransformation(edge.Data));
    return transformedGraph;
  }

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph()
  {
    return ToUnindexedGraph(data => data, data => data);
  }

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph(
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) =>
    TransformToUnindexedGraph(cloneNodeData, cloneEdgeData);

  public Graph<TNodeDataTransformed, TEdgeDataTransformed> TransformToUnindexedGraph<TNodeDataTransformed,
    TEdgeDataTransformed>(
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation)
  {
    var graph = new Graph<TNodeDataTransformed, TEdgeDataTransformed>();
    var nodeMap = nodes.Values.ToDictionary(node => node, node => graph.AddNode(nodeDataTransformation(node.Data)));
    foreach (var edge in edges)
      graph.AddEdge(nodeMap[edge.Origin], nodeMap[edge.Destination], edgeDataTransformation(edge.Data));
    return graph;
  }
}