namespace Graph;

public interface IUnindexedEdgeWriteModule<TNodeData, TEdgeData> :
  IEdgeWriteModule<TNodeData, TEdgeData>
{
  IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data);
}