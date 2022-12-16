namespace Graph;

public interface IWriteOnlyNodeAutoIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyNodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyNodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }