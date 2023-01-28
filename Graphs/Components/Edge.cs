namespace DataForge.Graphs;

public sealed class Edge<TNodeData, TEdgeData> :
  UnindexedGraphComponent<TNodeData, TEdgeData>,
  IEdge<TNodeData, TEdgeData>
{
  internal readonly Node<TNodeData, TEdgeData> InternalDestination;
  internal readonly Node<TNodeData, TEdgeData> InternalOrigin;

  private TEdgeData data;

  internal Edge(Graph<TNodeData, TEdgeData> graph, Node<TNodeData, TEdgeData> origin,
    Node<TNodeData, TEdgeData> destination, TEdgeData data) : base(graph)
  {
    InternalOrigin = origin;
    InternalDestination = destination;
    this.data = data;
  }

  public TEdgeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public Node<TNodeData, TEdgeData> Origin =>
    IsValid ? InternalOrigin : throw ComponentInvalidException;

  public Node<TNodeData, TEdgeData> Destination =>
    IsValid ? InternalDestination : throw ComponentInvalidException;

  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Origin => Origin;
  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Destination => Destination;

  public bool RemoveFromGraph() => IsValid && Graph.RemoveEdge(this);
}