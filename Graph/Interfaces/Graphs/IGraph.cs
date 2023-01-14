namespace Graph;

public interface IGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{  
  public bool RemoveNode(INode<TNodeData, TEdgeData> node);
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
  public int RemoveNodeWhere(Predicate<TNodeData> predicate);
  public int RemoveEdgeWhere(Predicate<TEdgeData> predicate);
  public void Clear();
}