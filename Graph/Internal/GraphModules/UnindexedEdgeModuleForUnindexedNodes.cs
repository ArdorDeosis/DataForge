namespace Graph;

internal sealed class UnindexedEdgeModuleForUnindexedNodes<TNodeData, TEdgeData> :
  GraphModule<uint, TNodeData, uint, TEdgeData>,
  IUnindexedEdgeCreator<TNodeData, TEdgeData>,
  IEdgeReader<TNodeData, TEdgeData>
{
  private uint edgeIndexCounter;

  public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => Graph.Edges;

  internal UnindexedEdgeModuleForUnindexedNodes(InternalGraph<uint, TNodeData, uint, TEdgeData> graph) : base(graph) { }

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) =>
    TryGetEdgeIndexInThisGraph(edge, out var index) && Graph.ContainsEdge(index);

  public IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data)
  {
    if (origin is null)
      throw new ArgumentNullException(nameof(origin));
    if (destination is null)
      throw new ArgumentNullException(nameof(destination));
    if (!TryGetNodeIndexInThisGraph(origin, out var originIndex))
      throw new InvalidOperationException("origin node is not part of this graph");
    if (!TryGetNodeIndexInThisGraph(origin, out var destinationIndex))
      throw new InvalidOperationException("destination node is not part of this graph");
    return Graph.AddEdge(edgeIndexCounter++, originIndex, destinationIndex, data);
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    TryGetEdgeIndexInThisGraph(edge, out var index) && Graph.RemoveEdge(index);
}