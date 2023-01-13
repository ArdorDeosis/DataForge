using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

public sealed class Graph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  #region Fields

  private readonly HashSet<Node<TNodeData, TEdgeData>> nodes;
  
  private readonly HashSet<Edge<TNodeData, TEdgeData>> edges;

  private readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    incomingEdges = new();

  private readonly MultiValueDictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>>
    outgoingEdges = new();

  #endregion

  #region Constructors
  
  public Graph()
  {
    nodes = new HashSet<Node<TNodeData, TEdgeData>>();
    edges = new HashSet<Edge<TNodeData, TEdgeData>>();
  }

  #endregion

  // TODO: should these be read-only collections?
  public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => nodes;
  public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => edges;

  public int Order => nodes.Count;
  public int Size => edges.Count;

  public bool Contains(INode<TNodeData, TEdgeData> node) => nodes.Contains(node);
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  // ### ADDITION & REMOVAL ###

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = new Node<TNodeData, TEdgeData>(this, data);
    nodes.Add(node);
    return node;
  }

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