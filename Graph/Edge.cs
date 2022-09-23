namespace Graph;

public class Edge<TNodeData, TEdgeData> : GraphComponent<TNodeData, TEdgeData>
{
  internal readonly Node<TNodeData, TEdgeData> Start;
  internal readonly Node<TNodeData, TEdgeData> End;

  internal IEnumerable<Node<TNodeData, TEdgeData>> Nodes
  {
    get
    {
      yield return Start;
      yield return End;
    }
  }

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

  public bool Contains(Node<TNodeData, TEdgeData> node) => Start == node || End == node;
}