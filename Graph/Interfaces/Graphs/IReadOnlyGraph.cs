namespace Graph;

public interface IReadOnlyGraph<TNodeData, TEdgeData>
{
  public IReadOnlyCollection<INode<TNodeData, TEdgeData>> Nodes { get; }
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get; }
  
  public bool Contains(INode<TNodeData, TEdgeData> node);
  public bool Contains(IEdge<TNodeData, TEdgeData> edge);
  
  public int Order { get; }
  public int Size { get; }
}