namespace Graph;

public abstract class IndexedGraphComponent<TIndex, TNodeData, TEdgeData> : GraphComponent
  where TIndex : notnull
{
  internal readonly IndexedGraph<TIndex, TNodeData, TEdgeData> Graph;

  private protected IndexedGraphComponent(IndexedGraph<TIndex, TNodeData, TEdgeData> graph)
  {
    Graph = graph ?? throw new ArgumentNullException(nameof(graph));
  }
}