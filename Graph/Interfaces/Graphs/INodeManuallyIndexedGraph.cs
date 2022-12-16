namespace Graph;

public interface INodeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IWriteOnlyNodeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }