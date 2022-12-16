namespace Graph;

public interface IEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }