namespace Graph;

public interface IGraph<TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  INodeRemover<TNodeData, TEdgeData>,
  IEdgeRemover<TNodeData, TEdgeData> { }