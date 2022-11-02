namespace Graph;

/// <summary>
/// A base class for a graph containing nodes and edges.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
public abstract class GraphBase<TNodeData, TEdgeData>
{
  /// <summary>
  /// Whether this graph contains the provided node.
  /// </summary>
  /// <param name="node">The node to check.</param>
  public abstract bool Contains(Node<TNodeData, TEdgeData> node);

  /// <summary>
  /// Whether the graph contains the provided edge.
  /// </summary>
  /// <param name="edge">The edge to check.</param>
  public abstract bool Contains(Edge<TNodeData, TEdgeData> edge);

  /// <summary>
  /// All nodes in this graph.
  /// </summary>
  public abstract IEnumerable<Node<TNodeData, TEdgeData>> Nodes { get; }

  /// <summary>
  /// All edges in this graph.
  /// </summary>
  public abstract IEnumerable<Edge<TNodeData, TEdgeData>> Edges { get; }

  /// <summary>
  /// Order of the graph. (Number of Nodes)
  /// </summary>
  public abstract int Order { get; }

  /// <summary>
  /// Size of the graph. (Number of Edges)
  /// </summary>
  public abstract int Size { get; }

  /// <summary>
  /// Creates a node with the provided data for this graph.
  /// </summary>
  /// <param name="data">The data the node will hold.</param>
  /// <returns>The created node.</returns>
  protected Node<TNodeData, TEdgeData> MakeNode(TNodeData data) => new(this, data);

  /// <summary>
  /// Creates an edge with the provided data for this graph, connecting the provided nodes.
  /// </summary>
  /// <param name="start">The node the edge comes from.</param>
  /// <param name="end">The node the edge goes to.</param>
  /// <param name="data">The data the edge will hold.</param>
  /// <returns>The created edge.</returns>
  /// <exception cref="ArgumentException">
  /// If either the <paramref name="start"/> node or the <paramref name="end"/> node are not part of this graph or are
  /// invalidated.
  /// </exception>
  protected Edge<TNodeData, TEdgeData> MakeEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data)
  {
    if (!start.IsIn(this))
      throw new ArgumentException("The start node is not part of this graph or has been invalidated.", nameof(start));
    if (!end.IsIn(this))
      throw new ArgumentException("The end node is not part of this graph or has been invalidated.", nameof(end));

    var edge = new Edge<TNodeData, TEdgeData>(this, start, end, data);
    start.InternalOutgoingEdgeList.Add(edge);
    end.InternalIncomingEdgeList.Add(edge);
    return edge;
  }
}