namespace Graph;

public interface ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>,
  IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }