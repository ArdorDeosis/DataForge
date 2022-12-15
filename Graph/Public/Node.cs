using System.Diagnostics.CodeAnalysis;

namespace Graph;

public sealed class Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponent<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
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

  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  internal bool TryGetIndex(out TNodeIndex index)
  {
    index = this.index;
    return IsValid;
  }
}