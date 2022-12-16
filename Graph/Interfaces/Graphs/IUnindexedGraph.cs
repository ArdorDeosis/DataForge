namespace Graph;

public interface IUnindexedGraph<TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IWriteOnlyUnindexedGraph<TNodeData, TEdgeData> { }