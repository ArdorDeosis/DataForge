using System.Diagnostics.CodeAnalysis;

namespace Graph;

public sealed class Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  GraphComponent<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>,
  ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  internal readonly TNodeIndex OriginIndex;
  internal readonly TNodeIndex DestinationIndex;

  private readonly TEdgeIndex index;
  private TEdgeData data;

  public TEdgeIndex Index => IsValid ? index : throw ComponentInvalidException;

  public TEdgeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin =>
    IsValid ? Graph.GetNode(OriginIndex) : throw ComponentInvalidException;

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination =>
    IsValid ? Graph.GetNode(DestinationIndex) : throw ComponentInvalidException;

  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Origin => Origin;
  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Destination => Destination;

  internal Edge(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph,
    TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data) : base(graph)
  {
    OriginIndex = origin;
    DestinationIndex = destination;
    this.index = index;
    this.data = data;
  }

  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  internal bool TryGetIndex(out TEdgeIndex index)
  {
    index = this.index;
    return IsValid;
  }
}