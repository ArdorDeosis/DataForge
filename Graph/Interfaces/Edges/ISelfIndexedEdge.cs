namespace Graph;

public interface ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData> : IEdge<TNodeData, TEdgeData>
{
  public TEdgeIndex Index { get; }
}