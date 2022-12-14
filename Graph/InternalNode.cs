namespace Graph;

public interface INode<TNodeData, TEdgeData>
{
  public IGraph<TNodeData, TEdgeData> Graph { get; }
  public TNodeData Data { get; set; }
  public bool IsValid { get; }
  internal void Invalidate();
}

internal sealed class InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : INode<TNodeData, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  internal InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph;
  public IGraph<TNodeData, TEdgeData> Graph => graph;
  public TNodeData Data { get; set; }
  internal TNodeIndex Index { get; }
  public bool IsValid { get; private set; } = true;

  internal InternalNode(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph, TNodeIndex index,
    TNodeData data)
  {
    this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
    Index = index;
    Data = data;
  }

  public void Invalidate() => IsValid = false;
}