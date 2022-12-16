namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IReadOnlyNodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IReadOnlyEdgeIndexedGraph<TNodeData, TEdgeIndex, TEdgeData>,
  IFullIndexedEdgeReader<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IFullyIndexedNodeReader<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull { }