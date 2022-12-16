namespace Graph;

public interface IWriteOnlyNodeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IWriteOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IManuallyIndexedNodeCreator<TNodeIndex, TNodeData>
  where TNodeIndex : notnull { }