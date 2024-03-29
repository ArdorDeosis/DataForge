﻿using JetBrains.Annotations;

namespace DataForge.Graphs;

[PublicAPI]
public sealed class IndexedEdge<TIndex, TNodeData, TEdgeData> :
  IndexedGraphComponent<TIndex, TNodeData, TEdgeData>, IIndexedEdge<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  internal readonly TIndex DestinationIndex;
  internal readonly TIndex OriginIndex;

  private TEdgeData data;

  internal IndexedEdge(IndexedGraph<TIndex, TNodeData, TEdgeData> graph, TIndex originIndex,
    TIndex destinationIndex, TEdgeData data) : base(graph)
  {
    OriginIndex = originIndex;
    DestinationIndex = destinationIndex;
    this.data = data;
  }

  protected override string Description => "Edge";

  public TEdgeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public IndexedNode<TIndex, TNodeData, TEdgeData> Origin =>
    IsValid ? Graph.GetNode(OriginIndex) : throw ComponentInvalidException;

  public IndexedNode<TIndex, TNodeData, TEdgeData> Destination =>
    IsValid ? Graph.GetNode(DestinationIndex) : throw ComponentInvalidException;

  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Origin => Origin;

  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Destination => Destination;
}