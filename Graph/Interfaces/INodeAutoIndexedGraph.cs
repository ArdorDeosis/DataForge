namespace Graph;

public interface INodeAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IAutoIndexedNodeCreator<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull { }