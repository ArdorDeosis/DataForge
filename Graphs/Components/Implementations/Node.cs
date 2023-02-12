using JetBrains.Annotations;

namespace DataForge.Graphs;

[PublicAPI]
public sealed class Node<TNodeData, TEdgeData> :
  UnindexedGraphComponent<TNodeData, TEdgeData>,
  INode<TNodeData, TEdgeData>
{
  private TNodeData data;

  internal Node(Graph<TNodeData, TEdgeData> graph, TNodeData data) : base(graph)
  {
    this.data = data;
  }

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges => IsValid
    ? Graph.IncomingEdges[this].Union(Graph.OutgoingEdges[this]).ToHashSet()
    : throw ComponentInvalidException;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> IncomingEdges => IsValid
    ? Graph.IncomingEdges[this]
    : throw ComponentInvalidException;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> OutgoingEdges => IsValid
    ? Graph.OutgoingEdges[this]
    : throw ComponentInvalidException;

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Neighbours => IsValid
    ? IncomingEdges.Select(edge => edge.Origin).Concat(OutgoingEdges.Select(edge => edge.Destination))
      .Where(node => node != this).ToHashSet()
    : throw ComponentInvalidException;

  protected override string Description => "Node";

  public TNodeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Edges => Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.IncomingEdges => IncomingEdges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.OutgoingEdges => OutgoingEdges;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Neighbours => Neighbours;
}