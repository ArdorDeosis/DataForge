namespace Graph;

public interface IGraph<TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>,
  INodeWriteModule<TNodeData, TEdgeData>,
  IEdgeWriteModule<TNodeData, TEdgeData> { }