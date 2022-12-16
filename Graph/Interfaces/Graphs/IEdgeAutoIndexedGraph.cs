namespace Graph;

public interface IEdgeAutoIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyEdgeAutoIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }