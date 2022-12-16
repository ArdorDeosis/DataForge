namespace Graph;

public interface IUnindexedGraph<TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IUnindexedNodeCreator<TNodeData, TEdgeData>,
  IUnindexedEdgeCreator<TNodeData, TEdgeData> { }