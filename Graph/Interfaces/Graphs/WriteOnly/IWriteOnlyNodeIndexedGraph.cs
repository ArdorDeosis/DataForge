namespace Graph;

public interface IWriteOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IWriteOnlyGraph,
  IIndexedNodeRemover<TNodeIndex, TNodeData, TEdgeData>,
  IUnindexedEdgeCreator<TNodeData, TEdgeData>,
  IEdgeRemover<TNodeData, TEdgeData>
  where TNodeIndex : notnull { }