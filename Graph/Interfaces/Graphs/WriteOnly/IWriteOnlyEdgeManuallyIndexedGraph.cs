namespace Graph;

public interface IWriteOnlyEdgeManuallyIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IManuallyIndexedEdgeCreator<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }