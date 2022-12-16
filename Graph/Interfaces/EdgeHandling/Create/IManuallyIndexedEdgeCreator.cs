namespace Graph;

public interface IManuallyIndexedEdgeCreator<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull
{
  void AddEdge(TEdgeIndex index, INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data);

  bool TryAddNode(TEdgeIndex index, INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data);
}