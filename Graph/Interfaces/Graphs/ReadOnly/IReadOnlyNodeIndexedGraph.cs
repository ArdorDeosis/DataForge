namespace Graph;

public interface IReadOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  INodeIndexedNodeReader<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }