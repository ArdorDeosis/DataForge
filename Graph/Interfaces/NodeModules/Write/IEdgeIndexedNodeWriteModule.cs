namespace Graph;

public interface IEdgeIndexedNodeWriteModule<TNodeData, TEdgeIndex, TEdgeData> :
  IUnindexedNodeWriteModule<TNodeData, TEdgeData> where TEdgeIndex : notnull
{
  new IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData> AddNode(TNodeData data);
}