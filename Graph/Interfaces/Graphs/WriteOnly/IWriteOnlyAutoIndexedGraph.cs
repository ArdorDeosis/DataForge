namespace Graph;

public interface IWriteOnlyAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyNodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyNodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }