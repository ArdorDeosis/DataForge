using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace DataForge.Graphs;

public sealed class IndexedGraph<TIndex, TNodeData, TEdgeData> :
  IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  #region Fields

  private readonly Dictionary<TIndex, IndexedNode<TIndex, TNodeData, TEdgeData>> nodes;
  private readonly HashSet<IndexedEdge<TIndex, TNodeData, TEdgeData>> edges;


  private readonly MultiValueDictionary<TIndex, IndexedEdge<TIndex, TNodeData, TEdgeData>>
    incomingEdges = new();

  private readonly MultiValueDictionary<TIndex, IndexedEdge<TIndex, TNodeData, TEdgeData>>
    outgoingEdges = new();

  private readonly Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod;

  #endregion

  #region Constructors

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

  #endregion

  #region Data Access

  // TODO: should these be read-only collections?
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes { get; }

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges { get; }

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public IReadOnlyCollection<TIndex> Indices { get; }

  public bool Contains(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && nodes.ContainsValue(indexedNode);

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  public bool Contains(TIndex index) => nodes.ContainsKey(index);

  public int Order => nodes.Count;
  public int Size => edges.Count;

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => nodes[index];

  // TODO: should these have custom exceptions?
  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => nodes[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) =>
    nodes.TryGetValue(index, out var node) ? node : null;

  public bool TryGetNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    nodes.TryGetValue(index, out node);

  #endregion

  #region Data Modification

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data)
  {
    if (nodes.ContainsKey(index))
      throw new InvalidOperationException($"Node with index {index} already exists.");
    var node = new IndexedNode<TIndex, TNodeData, TEdgeData>(this, index, data);
    nodes.Add(index, node);
    return node;
  }

  public bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    try
    {
      node = AddNode(index, data);
      return true;
    }
    catch (Exception _)
    {
      node = null;
      return false;
    }
  }

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination,
    TEdgeData data)
  {
    if (!nodes.ContainsKey(origin))
      throw new KeyNotFoundException("The origin node does not exist in the graph.");
    if (!nodes.ContainsKey(destination))
      throw new KeyNotFoundException("The destination node does not exist in the graph.");
    var edge = new IndexedEdge<TIndex, TNodeData, TEdgeData>(this, origin, destination, data);
    edges.Add(edge);
    outgoingEdges.Add(origin, edge);
    incomingEdges.Add(destination, edge);
    return edge;
  }

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge)
  {
    try
    {
      edge = AddEdge(origin, destination, data);
      return true;
    }
    catch (Exception _)
    {
      edge = default;
      return false;
    }
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

    foreach (var edge in incomingEdges[index].Concat(outgoingEdges[index]).Distinct())
      edges.Remove(edge);

    retrievedNode.Invalidate();
    return true;
  }

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node) =>
    node.Graph == this && node.IsValid && RemoveNode(node.Index);

  bool IGraph<TNodeData, TEdgeData>.RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is IndexedNode<TIndex, TNodeData, TEdgeData> indexedNode && RemoveNode(indexedNode);

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge)
  {
    if (!edges.Remove(edge)) return false;
    incomingEdges.RemoveFrom(edge.DestinationIndex, edge);
    outgoingEdges.RemoveFrom(edge.OriginIndex, edge);
    edge.Invalidate();
    return true;
  }

  bool IGraph<TNodeData, TEdgeData>.RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    edge is IndexedEdge<TIndex, TNodeData, TEdgeData> indexedEdge && RemoveEdge(indexedEdge);

  public int RemoveNodeWhere(Predicate<TNodeData> predicate) => nodes.Values
    .Where(node => predicate(node.Data))
    .Select(node => node.Index)
    .ToArray()
    .Select(RemoveNode)
    .Count();

  public int RemoveEdgeWhere(Predicate<TEdgeData> predicate) => edges.RemoveWhere(edge => predicate(edge.Data));

  public void Clear()
  {
    foreach (var node in nodes.Values)
      node.Invalidate();
    foreach (var edge in edges)
      edge.Invalidate();
    nodes.Clear();
    edges.Clear();
    incomingEdges.Clear();
    outgoingEdges.Clear();
  }

  #endregion

  public Graph<TNodeData, TEdgeData> ToUnindexedGraph()
  {
    var graph = new Graph<TNodeData, TEdgeData>();
    var nodeMap = nodes.Values.ToDictionary(node => node, node => graph.AddNode(node.Data));
    foreach (var edge in edges)
      graph.AddEdge(nodeMap[edge.Origin], nodeMap[edge.Destination], edge.Data);
    return graph;
  }

  public IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>
    Transform<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<TIndex, TIndexTransformed> indexTransformation
    ) where TIndexTransformed : notnull
  {
    var copy = new IndexedGraph<TIndexTransformed, TNodeDataTransformed, TEdgeDataTransformed>();
    var indexMap = nodes.Keys.ToDictionary(index => index, indexTransformation);
    foreach (var indexPair in indexMap)
      copy.AddNode(indexPair.Value, nodeDataTransformation(nodes[indexPair.Key].Data));
    foreach (var edge in edges)
      copy.AddEdge(indexMap[edge.Origin.Index], indexMap[edge.Origin.Index], edgeDataTransformation(edge.Data));
    return copy;
  }
}