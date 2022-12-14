namespace Graph;

public sealed class OldNode<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  public readonly TNodeData Data;

  private readonly InternalGraph<,,> graph;
  private readonly TIndex index;

  internal OldNode(InternalGraph<,,> graph, TIndex index, TNodeData data)
  {
    this.graph = graph;
    this.index = index;
    Data = data;
  }
}