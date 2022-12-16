namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  INodeIndexedNodeReader<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }