namespace Graph;

public interface IEdgeRemover<TNodeData, TEdgeData>
{
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
}