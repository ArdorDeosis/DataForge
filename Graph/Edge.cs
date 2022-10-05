namespace Graph;

/// <inheritdoc />
/// <summary>
/// The edge of a graph connecting two nodes and holding data.
/// </summary>
public class Edge<TNodeData, TEdgeData> : GraphComponent<TNodeData, TEdgeData>
{
  public readonly Node<TNodeData, TEdgeData> Start;
  public readonly Node<TNodeData, TEdgeData> End;

  /// <summary>
  /// A collection of both nodes this edge is connecting.
  /// </summary>
  internal IEnumerable<Node<TNodeData, TEdgeData>> Nodes
  {
    get
    {
      yield return Start;
      if (Start != End)
        yield return End;
    }
  }

  /// <summary>
  /// This edge's data.
  /// </summary>
  public TEdgeData Data { get; set; }

  internal Edge(GraphBase<TNodeData, TEdgeData> graph, Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data) : base(graph)
  {
    if (!start.IsIn(graph))
      throw new ArgumentException();
    if (!end.IsIn(graph))
      throw new ArgumentException();
    Start = start;
    End = end;
    Data = data;
  }

  /// <summary>
  /// Whether this edge goes from or to the provided node.
  /// </summary>
  /// <param name="node">The node to check.</param>
  public bool Contains(Node<TNodeData, TEdgeData> node) => Start == node || End == node;
}