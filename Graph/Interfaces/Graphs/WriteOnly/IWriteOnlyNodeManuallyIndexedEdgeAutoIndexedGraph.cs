namespace Graph;

public interface IWriteOnlyNodeManuallyIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyNodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyNodeManuallyIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }