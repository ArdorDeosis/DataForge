namespace Graph;

public interface IReadOnlyGraph<TNodeData, TEdgeData> :
  INodeReader<TNodeData, TEdgeData>,
  IEdgeReader<TNodeData, TEdgeData>
{
  public int Order { get; }
  public int Size { get; }
}