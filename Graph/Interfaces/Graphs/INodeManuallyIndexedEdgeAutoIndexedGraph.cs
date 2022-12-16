namespace Graph;

public interface INodeManuallyIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  INodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  INodeManuallyIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }