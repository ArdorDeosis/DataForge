namespace Graph;

public interface INodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IWriteOnlyNodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }