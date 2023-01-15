using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace DataForge.Graphs;

public sealed class Graph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  #region Fields

  private readonly HashSet<Node<TNodeData, TEdgeData>> nodes = new();

  private readonly HashSet<Edge<TNodeData, TEdgeData>> edges = new();

  private readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    incomingEdges = new();

  private readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    outgoingEdges = new();

  #endregion

  #region Constructors

  public Graph()
  {
    Nodes = nodes.InReadOnlyWrapper();
    Edges = edges.InReadOnlyWrapper();
  }

  #endregion

  #region Data Access

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Nodes { get; }
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges { get; }
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => Edges;

  public bool Contains(INode<TNodeData, TEdgeData> node) => nodes.Contains(node);
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  public int Order => nodes.Count;
  public int Size => edges.Count;

  #endregion

  #region Data Modification

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = new Node<TNodeData, TEdgeData>(this, data);
    nodes.Add(node);
    return node;
  }

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data) => data.Select(AddNode);

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination,
    TEdgeData data)
  {
    if (!nodes.Contains(origin))
      throw new KeyNotFoundException("The origin node does not exist in the graph.");
    if (!nodes.Contains(destination))
      throw new KeyNotFoundException("The destination node does not exist in the graph.");
    var edge = new Edge<TNodeData, TEdgeData>(this, origin, destination, data);
    edges.Add(edge);
    outgoingEdges.Add(origin, edge);
    incomingEdges.Add(destination, edge);
    return edge;
  }

  public bool TryAddEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge)
  {
    try
    {
      edge = AddEdge(start, end, data);
      return true;
    }
    catch (Exception _)
    {
      edge = default;
      return false;
    }
  }

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    node is Node<TNodeData, TEdgeData> castNode && nodes.Remove(castNode);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    edge is Edge<TNodeData, TEdgeData> castEdge && edges.Remove(castEdge);

  public int RemoveNodeWhere(Predicate<TNodeData> predicate) => nodes.RemoveWhere(node => predicate(node.Data));

  public int RemoveEdgeWhere(Predicate<TEdgeData> predicate) => edges.RemoveWhere(edge => predicate(edge.Data));

  public void Clear()
  {
    foreach (var node in nodes)
      node.Invalidate();
    foreach (var edge in edges)
      edge.Invalidate();
    nodes.Clear();
    edges.Clear();
    incomingEdges.Clear();
    outgoingEdges.Clear();
  }

  #endregion

  #region Transformation

  public Graph<TNodeData, TEdgeData> Clone() =>
    Transform(data => data, data => data);

  public Graph<TNodeData, TEdgeData> Clone(
    Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData) =>
    Transform(copyNodeData, copyEdgeData);

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

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider, IEqualityComparer<TIndex>? indexEqualityComparer = null)
    where TIndex : notnull
    => ToIndexedGraph(indexProvider, data => data, data => data, indexEqualityComparer);

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull
    => ToIndexedGraph(indexProvider, data => data, data => data, indexEqualityComparerFactoryMethod);

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(indexProvider, copyNodeData, copyEdgeData, indexEqualityComparer);

  public IndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    TransformToIndexedGraph(indexProvider, copyNodeData, copyEdgeData, indexEqualityComparerFactoryMethod);

  public IndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed> TransformToIndexedGraph<TIndex,
    TNodeDataTransformed, TEdgeDataTransformed>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
    Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    TransformToIndexedGraph(indexProvider, nodeDataTransformation, edgeDataTransformation, () => indexEqualityComparer);

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
      node => indexedGraph.AddNode(indexProvider.GetIndex(node.Data), nodeDataTransformation(node.Data)).Index
    );
    foreach (var edge in edges)
      indexedGraph.AddEdge(nodeMap[edge.Origin], nodeMap[edge.Destination], edgeDataTransformation(edge.Data));
    return indexedGraph;
  }

  #endregion
}