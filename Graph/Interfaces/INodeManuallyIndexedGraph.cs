namespace Graph;

public interface INodeManuallyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> :
  INodeIndexedGraph<TNodeIndex, TNodeData, TEdgeData>,
  IManuallyIndexedNodeCreator<TNodeIndex, TNodeData>
  where TNodeIndex : notnull { }