namespace Graph;

public interface IUnindexedNodeCreatorForIndexedEdges<TNodeData, TEdgeIndex, TEdgeData> :
  IUnindexedNodeCreator<TNodeData, TEdgeData> where TEdgeIndex : notnull
{
  new IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData> AddNode(TNodeData data);
}