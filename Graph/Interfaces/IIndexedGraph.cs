namespace Graph;

public interface IIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  ISelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>,
  IUnindexedEdgeWriteModule<TNodeData, TEdgeData>
  where TNodeIndex : notnull { }