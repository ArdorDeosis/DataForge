namespace Graph;

public interface INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IWriteOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }