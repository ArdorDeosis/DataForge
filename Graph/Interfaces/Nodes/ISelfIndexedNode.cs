namespace Graph;

public interface ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  public TNodeIndex Index { get; }
}