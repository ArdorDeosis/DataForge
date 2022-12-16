namespace Graph;

public interface INodeAutoIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  INodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  INodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }