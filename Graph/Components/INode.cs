namespace Graph;

public interface INode<TNodeData, out TEdgeData> : IGraphComponent
{
  public TNodeData Data { get; set; }
}