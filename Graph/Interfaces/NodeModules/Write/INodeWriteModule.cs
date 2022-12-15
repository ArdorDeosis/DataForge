namespace Graph;

public interface INodeWriteModule<TNodeData, TEdgeData>
{
  public bool RemoveNode(INode<TNodeData, TEdgeData> node);
}