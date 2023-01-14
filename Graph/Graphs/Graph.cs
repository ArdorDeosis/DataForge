using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

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

  public Graph(IReadOnlyCollection<Node<TNodeData, TEdgeData>> nodes, IReadOnlyCollection<Edge<TNodeData, TEdgeData>> edges)
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

  

  #endregion

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
}