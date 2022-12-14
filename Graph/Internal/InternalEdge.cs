namespace Graph;

internal sealed class
    InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex,
        TEdgeData>
    where TNodeIndex : notnull where TEdgeIndex : notnull
{
    internal readonly TEdgeIndex Index;
    internal readonly TNodeIndex Origin;
    internal readonly TNodeIndex Destination;

    internal TEdgeData Data;

    internal InternalEdge(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph,
        TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data) : base(graph)
    {
        Origin = origin;
        Destination = destination;
        Index = index;
        Data = data;
    }
}