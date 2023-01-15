namespace DataForge.Graphs;

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
}