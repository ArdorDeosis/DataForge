namespace Graph;

public interface IGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{
  public bool RemoveNode(INode<TNodeData, TEdgeData> node);
  public void AddEdge(INode<TNodeData, TEdgeData> start, INode<TNodeData, TEdgeData> end, TEdgeData data);
  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
}