namespace Graph;

public sealed class Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedEdge<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  private readonly TNodeIndex originIndex;
  private readonly TNodeIndex destinationIndex;

  public TEdgeIndex Index { get; }

  public TEdgeData Data { get; set; }
  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin => Graph.GetNode(originIndex);
  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination => Graph.GetNode(destinationIndex);

  internal Edge(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph,
    TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data) : base(graph)
  {
    originIndex = origin;
    destinationIndex = destination;
    Index = index;
    Data = data;
  }
}