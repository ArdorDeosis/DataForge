namespace DataForge.Graphs;

public abstract class UnindexedGraphComponent<TNodeData, TEdgeData> : GraphComponent
{
  internal readonly Graph<TNodeData, TEdgeData> Graph;

  private protected UnindexedGraphComponent(Graph<TNodeData, TEdgeData> graph)
  {
    Graph = graph ?? throw new ArgumentNullException(nameof(graph));
  }
}