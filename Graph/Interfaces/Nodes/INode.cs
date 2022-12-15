namespace Graph;

public interface INode<TNodeData, TEdgeData> : IGraphComponent
{
  public TNodeData Data { get; set; }
}