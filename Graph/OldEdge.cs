namespace Graph;

/// <summary>
/// The edge of a graph connecting two nodes and holding data.
/// </summary>
public class OldEdge<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  private readonly InternalGraph<,,> graph;
  internal readonly TIndex start;
  internal readonly TIndex end;
  private bool invalid;

  /// <summary>
  /// The node this edge comes from.
  /// </summary>
  public OldNode<TIndex, TNodeData, TEdgeData> Start
  {
    get
    {
      if (invalid) throw new InvalidOperationException();
      return graph[start];
    }
  }

  /// <summary>
  /// The node this edge goes to.
  /// </summary>
  public OldNode<TIndex, TNodeData, TEdgeData> End
  {
    get
    {
      if (invalid) throw new InvalidOperationException();
      return graph[end];
    }
  }

  public readonly TEdgeData Data;

  internal OldEdge(InternalGraph<,,> graph, TIndex start, TIndex end, TEdgeData data)
  {
    this.start = start;
    this.end = end;
    Data = data;
    this.graph = graph;
  }

  internal void Invalidate() => invalid = true;
}