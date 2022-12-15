namespace Graph;

public sealed class UnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  private readonly InternalGraph<uint, TNodeData, uint, TEdgeData> graph;
  private readonly UnindexedNodeModuleForUnindexedEdges<TNodeData, TEdgeData> nodeModule;
  private readonly UnindexedEdgeModuleForUnindexedNodes<TNodeData, TEdgeData> edgeModule;

  public UnindexedGraph()
  {
    graph = new InternalGraph<uint, TNodeData, uint, TEdgeData>();
    nodeModule = new UnindexedNodeModuleForUnindexedEdges<TNodeData, TEdgeData>(graph);
    edgeModule = new UnindexedEdgeModuleForUnindexedNodes<TNodeData, TEdgeData>(graph);
  }

  public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => nodeModule.Nodes;
  public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => edgeModule.Edges;
  public bool Contains(INode<TNodeData, TEdgeData> node) => nodeModule.Contains(node);

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => edgeModule.Contains(edge);

  public int Order => graph.Order;
  public int Size => graph.Size;
  public INode<TNodeData, TEdgeData> AddNode(TNodeData data) => nodeModule.AddNode(data);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) => nodeModule.RemoveNode(node);

  public IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data) =>
    edgeModule.AddEdge(origin, destination, data);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) => edgeModule.RemoveEdge(edge);
}