namespace Graph;

public interface IEdge<TNodeData, TEdgeData>
{
  IGraph<TNodeData, TEdgeData> Graph { get; }
  TEdgeData Data { get; set; }
  bool IsValid { get; }
  void Invalidate();
}

public sealed class InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : IEdge<TNodeData, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  public InternalGraph<TNodeIndex, TNodeData, TEdgeData> graph;
  public IGraph<TNodeData, TEdgeData> Graph => graph;
  public TEdgeData Data { get; set; }

  internal TEdgeIndex Index { get; }

  public TNodeIndex Origin { get; }
  public TNodeIndex Destination { get; }
  public bool IsValid { get; private set; } = true;


  internal InternalEdge(InternalGraph<TNodeIndex, TNodeData, TEdgeData> graph,
    TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data)
  {
    this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
    Origin = origin;
    Destination = destination;
    Data = data;
    Index = index;
  }

  public void Invalidate() => IsValid = false;
}