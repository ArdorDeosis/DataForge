namespace Graph;

public interface IReadOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  IEdgeIndexedEdgeReader<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull { }