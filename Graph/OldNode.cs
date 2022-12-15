namespace Graph;

public sealed class Node<TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
{
  private Node<uint, TNodeData, uint, TEdgeData> node;
  public IGraph<TNodeData, TEdgeData> Graph { get; }
  public bool IsValid => node.IsValid;

  public TNodeData Data
  {
    get => node.Data;
    set => node.Data = value;
  }

  private readonly InternalGraph<,,> graph;
  private readonly TIndex index;

  internal Node(IGraph<TNodeData, TEdgeData> graph, Node<uint, TNodeData, uint, TEdgeData> node)
  {
    Graph = graph;
    this.node = node;
  }
}