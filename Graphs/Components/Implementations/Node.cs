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

  public TNodeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges => IsValid
    ? Graph.IncomingEdges[this].Union(Graph.OutgoingEdges[this]).ToHashSet()
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Edges => Edges;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> IncomingEdges => IsValid
    ? Graph.IncomingEdges[this]
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.IncomingEdges => IncomingEdges;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> OutgoingEdges => IsValid
    ? Graph.OutgoingEdges[this]
    : throw ComponentInvalidException;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.OutgoingEdges => OutgoingEdges;

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Neighbours => IsValid
    ? IncomingEdges.Select(edge => edge.Origin).Concat(OutgoingEdges.Select(edge => edge.Destination))
      .Where(node => node != this).ToHashSet()
    : throw ComponentInvalidException;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> INode<TNodeData, TEdgeData>.Neighbours => Neighbours;
  protected override string Description => "Node";
}