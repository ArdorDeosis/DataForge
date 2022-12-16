namespace Graph;

internal sealed class UnindexedEdgeModuleForIndexedNodes<TNodeIndex, TNodeData, TEdgeData> :
  GraphModule<TNodeIndex, TNodeData, uint, TEdgeData>,
  IUnindexedEdgeCreatorForIndexedNodes<TNodeIndex, TNodeData, TEdgeData>,
  IEdgeRemover<TNodeData, TEdgeData>,
  INodeIndexedEdgeReader<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull


{
  private uint edgeIndexCounter;

  public IEnumerable<INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData>> Edges => Graph.Edges;

  IEnumerable<IEdge<TNodeData, TEdgeData>> IEdgeReader<TNodeData, TEdgeData>.Edges => Edges;

  internal UnindexedEdgeModuleForIndexedNodes(InternalGraph<TNodeIndex, TNodeData, uint, TEdgeData> graph) :
    base(graph) { }

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) =>
    TryGetEdgeIndexInThisGraph(edge, out var index) && Graph.ContainsEdge(index);

  public INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data)
  {
    if (origin is null)
      throw new ArgumentNullException(nameof(origin));
    if (destination is null)
      throw new ArgumentNullException(nameof(destination));
    if (!TryGetNodeIndexInThisGraph(origin, out var originIndex))
      throw new InvalidOperationException("origin node is not part of this graph");
    if (!TryGetNodeIndexInThisGraph(destination, out var destinationIndex))
      throw new InvalidOperationException("destination node is not part of this graph");
    return AddEdge(originIndex, destinationIndex, data);
  }

  IEdge<TNodeData, TEdgeData> IUnindexedEdgeCreator<TNodeData, TEdgeData>.AddEdge(
    INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination, TEdgeData data) =>
    AddEdge(origin, destination, data);

  public INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData> AddEdge(TNodeIndex origin, TNodeIndex destination,
    TEdgeData data) =>
    Graph.AddEdge(edgeIndexCounter++, origin, destination, data);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) =>
    TryGetEdgeIndexInThisGraph(edge, out var index) && Graph.RemoveEdge(index);
}