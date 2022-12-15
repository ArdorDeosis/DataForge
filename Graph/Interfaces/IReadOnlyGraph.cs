namespace Graph;

public interface IReadOnlyGraph<TNodeData, TEdgeData> :
  INodeReadModule<TNodeData, TEdgeData>,
  IEdgeReadModule<TNodeData, TEdgeData>
{
  public int Order { get; }
  public int Size { get; }
}