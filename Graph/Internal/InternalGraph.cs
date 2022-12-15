using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

internal sealed class InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  private readonly Dictionary<TNodeIndex, Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> nodes;
  private readonly Dictionary<TEdgeIndex, Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> edges;

  private readonly HashSetDictionary<TNodeIndex, TEdgeIndex>
    outgoingEdges = new();

  private readonly HashSetDictionary<TNodeIndex, TEdgeIndex>
    incomingEdges = new();

  private readonly Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod;
  private readonly Func<IEqualityComparer<TEdgeIndex>?> edgeIndexEqualityComparerFactoryMethod;

  internal IEnumerable<TNodeIndex> NodeIndices => nodes.Keys;
  internal IEnumerable<TEdgeIndex> EdgeIndices => edges.Keys;

  // TODO: should these be read-only collections?
  internal IEnumerable<Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Nodes => nodes.Values;
  internal IEnumerable<Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Edges => edges.Values;

  // TODO: should these have custom exceptions?
  internal Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetNode(TNodeIndex index) => nodes[index];
  internal Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetEdge(TEdgeIndex index) => edges[index];

  internal Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? GetNodeOrNull(TNodeIndex index) =>
    nodes.TryGetValue(index, out var node) ? node : null;

  internal Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? GetEdgeOrNull(TEdgeIndex index) =>
    edges.TryGetValue(index, out var edge) ? edge : null;

  internal bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node) =>
    nodes.TryGetValue(index, out node);

  internal bool TryGetEdge(TEdgeIndex index,
    [NotNullWhen(true)] out Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? edge) =>
    edges.TryGetValue(index, out edge);

  internal int Order => nodes.Count;
  internal int Size => edges.Count;

  internal bool ContainsNode(TNodeIndex index) => nodes.ContainsKey(index);
  internal bool ContainsEdge(TEdgeIndex index) => edges.ContainsKey(index);

  // ### CONSTRUCTORS ###

  internal InternalGraph() : this(() => null, () => null) { }

  internal InternalGraph(IEqualityComparer<TNodeIndex>? nodeIndexEqualityComparer,
    IEqualityComparer<TEdgeIndex>? edgeIndexEqualityComparer)
    : this(() => nodeIndexEqualityComparer, () => edgeIndexEqualityComparer) { }

  internal InternalGraph(Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod,
    Func<IEqualityComparer<TEdgeIndex>?> edgeIndexEqualityComparerFactoryMethod)
  {
    this.nodeIndexEqualityComparerFactoryMethod = nodeIndexEqualityComparerFactoryMethod;
    this.edgeIndexEqualityComparerFactoryMethod = edgeIndexEqualityComparerFactoryMethod;
    nodes = new Dictionary<TNodeIndex, Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>(
      nodeIndexEqualityComparerFactoryMethod());
    edges = new Dictionary<TEdgeIndex, Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>(
      edgeIndexEqualityComparerFactoryMethod());
  }

  private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin)
    : this(origin, nodeData => nodeData, edgeData => edgeData)
  {
    // TODO
  }

  private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin,
    Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData)
  {
    // TODO
    throw new NotImplementedException();
  }

  // internal static InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  //   Transform<TOriginalNodeIndex, TOriginalNodeData, TOriginalEdgeIndex, TOriginalEdgeData>(
  //     InternalGraph<TOriginalNodeIndex, TOriginalNodeData, TOriginalEdgeData> origin,
  //     Func<TOriginalNodeIndex, TNodeIndex> transformNodeIndex,
  //     Func<TOriginalNodeData, TNodeData> transformNodeData,
  //     Func<TOriginalEdgeData, TEdgeData> transformEdgeData,
  //     Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod)
  //   where TOriginalNodeIndex : notnull where TOriginalEdgeIndex : notnull
  // {
  //   // TODO
  //   throw new NotImplementedException();
  // }

  // ### ADDITION & REMOVAL ###

  internal Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddNode(TNodeIndex index,
    TNodeData data)
  {
    if (nodes.ContainsKey(index))
      throw new InvalidOperationException($"Node with index {index} already exists.");
    var node = new Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, index, data);
    nodes.Add(index, node);
    return node;
  }

  internal bool TryAddNode(TNodeIndex index, TNodeData data,
    [NotNullWhen(true)] out Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node)
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

  internal bool RemoveNode(TNodeIndex index) => RemoveNode(index, out _);

  internal bool RemoveNode(TNodeIndex index,
    [NotNullWhen(true)] out Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node)
  {
    if (!nodes.Remove(index, out var retrievedNode))
    {
      node = null;
      return false;
    }

    node = retrievedNode;
    var edgeRemovalActions = incomingEdges[index].Concat(outgoingEdges[index])
      .Distinct().Select<TEdgeIndex, Action>(edge => () => RemoveEdge(edge));
    foreach (var action in edgeRemovalActions)
      action();
    retrievedNode.Invalidate();
    return true;
  }

  internal Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddEdge(TEdgeIndex index,
    TNodeIndex origin, TNodeIndex destination, TEdgeData data)
  {
    if (!nodes.ContainsKey(origin))
      throw new KeyNotFoundException("The origin node does not exist in the graph.");
    if (!nodes.ContainsKey(destination))
      throw new KeyNotFoundException("The destination node does not exist in the graph.");
    var edge = new Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, index, origin, destination, data);
    edges.Add(index, edge);
    outgoingEdges.Add(origin, index);
    incomingEdges.Add(destination, index);
    return edge;
  }

  internal bool TryAddEdge(TEdgeIndex index, TNodeIndex start, TNodeIndex end, TEdgeData data)
  {
    try
    {
      AddEdge(index, start, end, data);
      return true;
    }
    catch (Exception _)
    {
      return false;
    }
  }

  internal bool RemoveEdge(TEdgeIndex index) => RemoveEdge(index, out _);

  internal bool RemoveEdge(TEdgeIndex index,
    [NotNullWhen(true)] out Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? edge)
  {
    if (!edges.Remove(index, out var retrievedEdge))
    {
      edge = null;
      return false;
    }

    edge = retrievedEdge;
    outgoingEdges.RemoveFrom(retrievedEdge.OriginIndex, index);
    incomingEdges.RemoveFrom(retrievedEdge.DestinationIndex, index);
    retrievedEdge.Invalidate();
    return true;
  }
}