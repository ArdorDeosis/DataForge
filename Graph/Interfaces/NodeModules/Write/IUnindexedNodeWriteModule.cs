namespace Graph;

public interface IUnindexedNodeWriteModule<TNodeData, TEdgeData> :
  INodeWriteModule<TNodeData, TEdgeData>
{
  INode<TNodeData, TEdgeData> AddNode(TNodeData data);
}