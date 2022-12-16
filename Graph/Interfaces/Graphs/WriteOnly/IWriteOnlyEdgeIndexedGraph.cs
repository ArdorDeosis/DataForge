namespace Graph;

public interface IWriteOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyGraph,
  IIndexedEdgeRemover<TNodeData, TEdgeIndex, TEdgeData>,
  IUnindexedNodeCreator<TNodeData, TEdgeData>,
  INodeRemover<TNodeData, TEdgeData>
  where TEdgeIndex : notnull { }