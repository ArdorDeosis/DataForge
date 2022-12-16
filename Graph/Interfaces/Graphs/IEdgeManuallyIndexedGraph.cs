namespace Graph;

public interface IEdgeManuallyIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyEdgeManuallyIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }