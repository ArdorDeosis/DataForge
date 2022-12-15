namespace Graph;

public interface IAutoSelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData> :
  ISelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  TNodeIndex AddNode(TNodeData data);
  bool TryAddNode(TNodeData data, out TNodeIndex index);
}