namespace Graph;

public interface IWriteOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyGraph,
  IIndexedEdgeRemover<TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedNodeRemover<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }