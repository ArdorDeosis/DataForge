namespace Graph;

public interface IWriteOnlyNodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IManuallyIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }