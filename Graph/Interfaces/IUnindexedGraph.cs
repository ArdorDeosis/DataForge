namespace Graph;

public interface IUnindexedGraph<TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IUnindexedNodeWriteModule<TNodeData, TEdgeData>,
  IUnindexedEdgeWriteModule<TNodeData, TEdgeData> { }