namespace Graph;

public interface INodeRemover<TNodeData, TEdgeData>
{
  public bool RemoveNode(INode<TNodeData, TEdgeData> node);
}