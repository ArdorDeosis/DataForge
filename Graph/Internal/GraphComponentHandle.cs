namespace Graph;

internal abstract class GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> where TNodeIndex : notnull where TEdgeIndex : notnull
{
    internal InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> Graph;
    internal bool IsValid { get; private set; } = true;
    internal void Invalidate() => IsValid = false;
    
    protected GraphComponentHandle(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph)
    {
        Graph = graph ?? throw new ArgumentNullException(nameof(graph));
    }

    protected InvalidOperationException ComponentInvalidException =>
        new("This graph component has been removed from its graph.");
}