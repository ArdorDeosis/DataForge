namespace Graph;

public interface IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData> : INode<TNodeData, TEdgeData>
  where TEdgeIndex : notnull { }