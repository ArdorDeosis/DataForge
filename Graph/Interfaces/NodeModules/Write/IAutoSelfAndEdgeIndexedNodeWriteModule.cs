namespace Graph;

public interface IAutoSelfAndEdgeIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IAutoSelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>,
  ISelfAndEdgeIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  new ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> AddNode(TNodeData data);
}