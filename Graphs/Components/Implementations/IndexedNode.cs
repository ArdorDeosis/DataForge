using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace DataForge.Graphs;

[PublicAPI]
public sealed class IndexedNode<TIndex, TNodeData, TEdgeData> :
  IndexedGraphComponent<TIndex, TNodeData, TEdgeData>, 
  IIndexedNode<TIndex, TNodeData, TEdgeData> 
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
  public bool TryGetIndex(out TIndex index)
  {
    index = this.index;
    return IsValid;
  }

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges => IsValid
    ? Graph.IncomingEdges[index].Union(Graph.OutgoingEdges[index]).ToHashSet()
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Edges => Edges;

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> IncomingEdges => IsValid
    ? Graph.IncomingEdges[index]
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.IncomingEdges => IncomingEdges;

  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> OutgoingEdges => IsValid
    ? Graph.OutgoingEdges[index]
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.OutgoingEdges => OutgoingEdges;

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Neighbours => IsValid
    ? IncomingEdges.Select(edge => edge.Origin).Concat(OutgoingEdges.Select(edge => edge.Destination))
      .Where(node => node != this).ToHashSet()
    : throw ComponentInvalidException;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Neighbours => Neighbours;

  protected override string Description => "Node";
}