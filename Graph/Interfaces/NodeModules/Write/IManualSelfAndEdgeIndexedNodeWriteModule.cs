namespace Graph;

public interface IManualSelfAndEdgeIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IManualSelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>,
  ISelfAndEdgeIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  new ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddNode(TNodeIndex index, TNodeData data);

  bool TryAddNode(TNodeIndex index, TNodeData data,
    out ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> node);
}