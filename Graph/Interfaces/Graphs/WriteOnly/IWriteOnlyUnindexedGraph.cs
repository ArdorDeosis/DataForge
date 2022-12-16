namespace Graph;

public interface IWriteOnlyUnindexedGraph<TNodeData, TEdgeData> :
  IWriteOnlyGraph,
  IUnindexedNodeCreator<TNodeData, TEdgeData>,
  IUnindexedEdgeCreator<TNodeData, TEdgeData>,
  INodeRemover<TNodeData, TEdgeData>,
  IEdgeRemover<TNodeData, TEdgeData> { }