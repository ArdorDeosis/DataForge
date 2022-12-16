namespace Graph;

public interface IIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IWriteOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }