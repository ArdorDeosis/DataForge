namespace Graph;

public interface IAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  INodeIndexedEdgeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  INodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }