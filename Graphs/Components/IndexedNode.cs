using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

public sealed class IndexedNode<TIndex, TNodeData, TEdgeData> :
  IndexedGraphComponent<TIndex, TNodeData, TEdgeData>,
  INode<TNodeData, TEdgeData>
  where TIndex : notnull
{
  private readonly TIndex index;
  private TNodeData data;

  internal IndexedNode(IndexedGraph<TIndex, TNodeData, TEdgeData> graph, TIndex index,
    TNodeData data) : base(graph)
  {
    this.index = index;
    this.data = data;
  }

  public TIndex Index => IsValid ? index : throw ComponentInvalidException;


  public TNodeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  internal bool TryGetIndex(out TIndex index)
  {
    index = this.index;
    return IsValid;
  }

  public override bool RemoveFromGraph() => IsValid && Graph.RemoveNode(Index);
}