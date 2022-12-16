namespace Graph;

internal sealed class UnindexedNodeModuleForUnindexedEdges<TNodeData, TEdgeData> :
  GraphModule<uint, TNodeData, uint, TEdgeData>,
  IUnindexedNodeCreator<TNodeData, TEdgeData>,
  INodeReader<TNodeData, TEdgeData>
{
  private uint nodeIndexCounter;

  public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => Graph.Nodes;

  internal UnindexedNodeModuleForUnindexedEdges(InternalGraph<uint, TNodeData, uint, TEdgeData> graph) : base(graph) { }

  public bool Contains(INode<TNodeData, TEdgeData> node) =>
    TryGetNodeIndexInThisGraph(node, out var index) && Graph.ContainsNode(index);

  public INode<TNodeData, TEdgeData> AddNode(TNodeData data) => Graph.AddNode(nodeIndexCounter++, data);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    TryGetNodeIndexInThisGraph(node, out var index) && Graph.RemoveNode(index);
}