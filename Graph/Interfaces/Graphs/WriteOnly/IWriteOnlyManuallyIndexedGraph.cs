namespace Graph;

public interface IWriteOnlyManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyNodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyNodeManuallyIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }