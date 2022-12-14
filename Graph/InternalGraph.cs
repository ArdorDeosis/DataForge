using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

internal sealed class InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  private readonly Dictionary<TNodeIndex, InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> nodes;
  private readonly Dictionary<TEdgeIndex, InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> edges;

  private readonly HashSetDictionary<TNodeIndex, InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>
    outgoingEdges = new();

  private readonly HashSetDictionary<TNodeIndex, InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>
    incomingEdges = new();

  private readonly Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod;
  private readonly Func<IEqualityComparer<TEdgeIndex>?> edgeIndexEqualityComparerFactoryMethod;

  internal IEnumerable<TNodeIndex> NodeIndices => nodes.Keys;
  internal IEnumerable<TEdgeIndex> EdgeIndices => edges.Keys;

  // TODO: should these be read-only collections?
  public IEnumerable<InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Nodes => nodes.Values;
  public IEnumerable<InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Edges => edges.Values;

  // TODO: should these have custom exceptions?
  public InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetNode(TNodeIndex index) => nodes[index];
  public InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetEdge(TEdgeIndex index) => edges[index];

  public bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node) =>
    nodes.TryGetValue(index, out node);

  public bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index)
  {
    if (node is InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> indexedNode &&
        nodes.TryGetFirst(pair => pair.Value == node, out var pair))
    {
      index = pair.Key;
      return true;
    }

    index = default;
    return false;
  }

  public int Order => nodes.Count;
  public int Size => edges.Count;

  public bool Contains(TNodeIndex index) => nodes.ContainsKey(index);

  // ### CONSTRUCTORS ###

  public InternalGraph() : this(() => null) { }

  public InternalGraph(IEqualityComparer<TNodeIndex>? nodeIndexEqualityComparer)
    : this(() => nodeIndexEqualityComparer) { }

  public InternalGraph(Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    this.nodeIndexEqualityComparerFactoryMethod = nodeIndexEqualityComparerFactoryMethod;
    nodes = new Dictionary<TNodeIndex, InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>(
      nodeIndexEqualityComparerFactoryMethod());
  }

  private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin)
    : this(origin, nodeData => nodeData, edgeData => edgeData) { }

  private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin,
    Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData)
  {
    // TODO
    throw new NotImplementedException();
  }

  // public static InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
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

  public void AddNode(TNodeIndex index, TNodeData data)
  {
    if (nodes.ContainsKey(index))
      throw new InvalidOperationException($"Node with index {index} already exists.");
    nodes.Add(index, new InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, data));
  }

  public bool TryAddNode(TNodeIndex index, TNodeData data)
  {
    if (nodes.ContainsKey(index))
      return false;
    nodes.Add(index, new InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, data));
    return true;
  }

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> indexedNode &&
    nodes.TryGetFirst(pair => pair.Value == indexedNode, out var pair) &&
    RemoveNode(pair.Key);

  public bool RemoveNode(TNodeIndex index)
  {
    if (!nodes.Remove(index))
      return false;
    var edgeRemovalActions = Enumerable.Concat(incomingEdges[index], outgoingEdges[index])
      .ToHashSet()
      .Select<InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>, Action>(edge => () => RemoveEdge(edge));
    foreach (var action in edgeRemovalActions)
      action();
    return true;
  }

  public void AddEdge(TNodeIndex start, TNodeIndex end, TEdgeData data)
  {
    if (!nodes.ContainsKey(start))
      throw new KeyNotFoundException("The start node does not exist in the graph.");
    if (!nodes.ContainsKey(end))
      throw new KeyNotFoundException("The end node does not exist in the graph.");
    var edge = new InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, start, end, data);
    edges.Add(edge);
    outgoingEdges.Add(start, edge);
    incomingEdges.Add(end, edge);
  }

  public void AddEdge(INode<TNodeData, TEdgeData> start, INode<TNodeData, TEdgeData> end, TEdgeData data)
  {
    if (!TryGetNodeIndex(start, out var startIndex))
      throw new Exception();
    if (!TryGetNodeIndex(end, out var endIndex))
      throw new Exception();
    AddEdge(startIndex, endIndex, data);
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    if (edge is not InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> indexedGraphEdge)
      return false;
    if (!edges.Remove(indexedGraphEdge))
      return false;
    outgoingEdges.RemoveFrom(indexedGraphEdge.Origin, indexedGraphEdge);
    incomingEdges.RemoveFrom(indexedGraphEdge.Destination, indexedGraphEdge);
    return true;
  }
}