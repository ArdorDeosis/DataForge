namespace Graph;

public interface IEdgeWriteModule<TNodeData, TEdgeData>
{
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
}