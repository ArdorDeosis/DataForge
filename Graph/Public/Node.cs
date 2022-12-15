namespace Graph;

public sealed class Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  private readonly TNodeIndex index;
  private TNodeData data;

  public TNodeIndex Index => IsValid ? index : throw ComponentInvalidException;

  public TNodeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  internal Node(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph, TNodeIndex index,
    TNodeData data) : base(graph)
  {
    this.index = index;
    this.data = data;
  }
}