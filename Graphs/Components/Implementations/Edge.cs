using JetBrains.Annotations;

namespace DataForge.Graphs;

[PublicAPI]
public sealed class Edge<TNodeData, TEdgeData> :
  UnindexedGraphComponent<TNodeData, TEdgeData>,
  IEdge<TNodeData, TEdgeData>
{
  private readonly Node<TNodeData, TEdgeData> destination;
  private readonly Node<TNodeData, TEdgeData> origin;

  private TEdgeData data;

  internal Edge(Graph<TNodeData, TEdgeData> graph, Node<TNodeData, TEdgeData> origin,
    Node<TNodeData, TEdgeData> destination, TEdgeData data) : base(graph)
  {
    this.origin = origin;
    this.destination = destination;
    this.data = data;
  }

  public TEdgeData Data
  {
    get => data;
    set
    {
      if (!IsValid) throw DataImmutableException;
      data = value;
    }
  }

  public Node<TNodeData, TEdgeData> Origin =>
    IsValid ? origin : throw ComponentInvalidException;

  public Node<TNodeData, TEdgeData> Destination =>
    IsValid ? destination : throw ComponentInvalidException;

  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Origin => Origin;
  
  INode<TNodeData, TEdgeData> IEdge<TNodeData, TEdgeData>.Destination => Destination;
  
  protected override string Description => "Edge";
}