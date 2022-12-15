namespace Graph;

public interface IAutoSelfAndNodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IAutoSelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData>,
  ISelfAndNodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  TEdgeIndex AddNode(TNodeIndex origin, TNodeIndex destination, TEdgeData data);

  bool TryAddNode(TNodeIndex origin, TNodeIndex destination, TEdgeData data, out TEdgeData index);
}