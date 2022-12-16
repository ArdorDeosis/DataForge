namespace Graph;

public interface IManuallyIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IManuallyIndexedEdgeCreator<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  void AddEdge(TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data);

  bool TryAddNode(TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data);
}