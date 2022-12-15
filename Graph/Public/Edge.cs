namespace Graph;

public sealed class Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedEdge<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  internal readonly TNodeIndex OriginIndex;
  internal readonly TNodeIndex DestinationIndex;

  private readonly TEdgeIndex index;
  private TEdgeData data;

  public TEdgeIndex Index => IsValid ? index : throw ComponentInvalidException;

  public TEdgeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin =>
    IsValid ? Graph.GetNode(OriginIndex) : throw ComponentInvalidException;

  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination =>
    IsValid ? Graph.GetNode(DestinationIndex) : throw ComponentInvalidException;

  internal Edge(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph,
    TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data) : base(graph)
  {
    OriginIndex = origin;
    DestinationIndex = destination;
    this.index = index;
    this.data = data;
  }
}