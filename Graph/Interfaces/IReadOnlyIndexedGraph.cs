namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  ISelfIndexedNodeReadModule<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }