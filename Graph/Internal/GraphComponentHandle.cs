namespace Graph;

public abstract class GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IGraphComponent
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  public bool IsValid { get; private set; } = true;

  private protected InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> Graph;

  internal void Invalidate() => IsValid = false;

  private protected GraphComponentHandle(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph)
  {
    Graph = graph ?? throw new ArgumentNullException(nameof(graph));
  }
}