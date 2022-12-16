namespace Graph;

public interface IUnindexedEdgeCreator<TNodeData, TEdgeData>
{
  IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data);
}