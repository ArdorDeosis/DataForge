namespace Graph;

public interface IGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{  
  public bool RemoveNode(INode<TNodeData, TEdgeData> node);
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
  public void Clear();
}