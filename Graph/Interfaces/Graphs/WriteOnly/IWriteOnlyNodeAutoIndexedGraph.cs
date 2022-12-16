namespace Graph;

public interface IWriteOnlyNodeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IWriteOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IAutoIndexedNodeCreator<TNodeIndex, TNodeData>
  where TNodeIndex : notnull { }