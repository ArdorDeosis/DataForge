namespace Graph;

public interface ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData>,
  INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }