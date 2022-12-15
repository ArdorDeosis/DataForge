namespace Graph;

public interface IEdge<TNodeData, TEdgeData> : IGraphComponent
{
  TEdgeData Data { get; set; }
  INode<TNodeData, TEdgeData> Origin { get; }
  INode<TNodeData, TEdgeData> Destination { get; }
}