namespace Graph;

public interface INodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeData> :
  IUnindexedEdgeWriteModule<TNodeData, TEdgeData> where TNodeIndex : notnull
{
  new INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data);

  INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData> AddEdge(TNodeIndex origin, TNodeIndex destination, TEdgeData data);
}