namespace Graph;

public abstract class GraphBase<TNodeData, TEdgeData>
{
  public abstract bool Contains(Node<TNodeData, TEdgeData> node);

  public abstract bool Contains(Edge<TNodeData, TEdgeData> edge);

  public abstract IEnumerable<Node<TNodeData, TEdgeData>> Nodes { get; }

  public abstract IEnumerable<Edge<TNodeData, TEdgeData>> Edges { get; }

  protected Node<TNodeData, TEdgeData> MakeNode(TNodeData data) => new(this, data);

  protected Edge<TNodeData, TEdgeData> MakeEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data)
  {
    if (!start.IsIn(this))
      throw new InvalidOperationException();
    if (!end.IsIn(this))
      throw new InvalidOperationException();

    var edge = new Edge<TNodeData, TEdgeData>(this, start, end, data);
    start.InternalOutgoingEdgeList.Add(edge);
    end.InternalIncomingEdgeList.Add(edge);
    return edge;
  }
}