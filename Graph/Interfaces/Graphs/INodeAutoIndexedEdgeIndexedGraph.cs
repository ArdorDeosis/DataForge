namespace Graph;

public interface INodeAutoIndexedEdgeIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IAutoIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }