namespace Graph;

public interface INodeIndexedEdgeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IManuallyIndexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }