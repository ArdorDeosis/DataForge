namespace Graph;

public interface IAutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  TNodeIndex AddNode(TNodeData data);
  bool RemoveNode(TNodeIndex index);
  void AddEdge(TNodeIndex start, TNodeIndex end, TEdgeData data);
}