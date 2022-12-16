namespace Graph;

public interface IWriteOnlyNodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IAutoIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }