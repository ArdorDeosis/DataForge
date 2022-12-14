namespace Graph;

public sealed class Graph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
{
  private readonly InternalGraph<uint, TNodeData, TEdgeData> graph = new();
  private uint nodeIndexCounter;

  public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => graph.Nodes;
  public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => graph.Edges;
  public int Order => graph.Order;
  public int Size => graph.Size;

  public void AddNode(TNodeData data) => graph.AddNode(nodeIndexCounter++, data);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) => graph.RemoveNode(node);

  public void AddEdge(INode<TNodeData, TEdgeData> start, INode<TNodeData, TEdgeData> end, TEdgeData data) =>
    graph.AddEdge(start, end, data);

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) => graph.RemoveEdge(edge);
}