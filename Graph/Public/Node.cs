namespace Graph;

public sealed class Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  public TNodeIndex Index { get; }
  public TNodeData Data { get; set; }

  internal Node(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph, TNodeIndex index,
    TNodeData data) : base(graph)
  {
    Index = index;
    Data = data;
  }
}