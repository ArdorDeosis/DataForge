namespace Graph;

public interface INodeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IWriteOnlyNodeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }