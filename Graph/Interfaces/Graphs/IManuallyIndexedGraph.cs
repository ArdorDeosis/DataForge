namespace Graph;

public interface IManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  INodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  INodeManuallyIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }