namespace Graph;

public interface INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData> : IEdge<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  new ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin { get; }
  new ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination { get; }
}