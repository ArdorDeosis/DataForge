namespace Graph;

public interface INodeManuallyIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IManuallyIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }