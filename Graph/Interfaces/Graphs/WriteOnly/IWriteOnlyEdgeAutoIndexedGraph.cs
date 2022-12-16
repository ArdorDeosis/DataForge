namespace Graph;

public interface IWriteOnlyEdgeAutoIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IAutoIndexedEdgeCreator<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }