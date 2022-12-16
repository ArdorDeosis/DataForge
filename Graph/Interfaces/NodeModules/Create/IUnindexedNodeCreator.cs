namespace Graph;

public interface IUnindexedNodeCreator<TNodeData, TEdgeData>
{
  INode<TNodeData, TEdgeData> AddNode(TNodeData data);
}