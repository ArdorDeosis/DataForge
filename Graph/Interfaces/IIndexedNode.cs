namespace Graph;

public interface IIndexedNode<TNodeIndex, TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
{
  public TNodeIndex Index { get; }
}