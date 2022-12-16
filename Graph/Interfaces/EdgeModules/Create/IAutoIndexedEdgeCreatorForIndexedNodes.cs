namespace Graph;

public interface IAutoIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IAutoIndexedEdgeCreator<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  TEdgeIndex AddNode(TNodeIndex origin, TNodeIndex destination, TEdgeData data);

  bool TryAddNode(TNodeIndex origin, TNodeIndex destination, TEdgeData data, out TEdgeData index);
}