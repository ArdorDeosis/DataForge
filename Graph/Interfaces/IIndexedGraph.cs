namespace Graph;

public interface INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IIndexedNodeRemover<TNodeIndex, TNodeData, TEdgeData>,
  IUnindexedEdgeCreator<TNodeData, TEdgeData>
  where TNodeIndex : notnull { }