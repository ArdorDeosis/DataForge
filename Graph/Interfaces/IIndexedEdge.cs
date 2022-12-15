namespace Graph;

public interface IIndexedEdge<TNodeIndex, TNodeData, TEdgeData> : IGraphComponent
{
  TEdgeData Data { get; set; }
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin { get; }
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination { get; }
}