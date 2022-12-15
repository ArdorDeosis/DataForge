namespace Graph;

public interface IManualSelfAndNodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IManualSelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData>,
  ISelfAndNodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  new ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddEdge(TEdgeIndex index,
    INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data);

  bool TryAddNode(TEdgeIndex index, INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data, out ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> edge);

  new ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddEdge(TEdgeIndex index, TNodeIndex origin,
    TNodeIndex destination, TEdgeData data);

  bool TryAddNode(TEdgeIndex index, TNodeIndex origin, TNodeIndex destination,
    TEdgeData data, out ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> edge);
}