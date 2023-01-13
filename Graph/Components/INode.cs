namespace Graph;

public interface INode<out TNodeData, out TEdgeData> : IGraphComponent
{
  public TNodeData Data { get; }
}