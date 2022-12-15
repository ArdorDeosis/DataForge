namespace Graph;

public interface IManualSelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull
{
  ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData> AddEdge(TEdgeIndex index, INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data);

  bool TryAddNode(TEdgeIndex index, INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data, out ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData> edge);
}