namespace Graph;

internal sealed class
    InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex,
        TEdgeData>
    where TNodeIndex : notnull where TEdgeIndex : notnull
{
    private readonly TEdgeIndex index;
    internal TEdgeIndex Index => IsValid ? index : throw ComponentInvalidException;
    private readonly TNodeIndex origin;
    internal TNodeIndex Origin => IsValid ? origin : throw ComponentInvalidException;
    private readonly TNodeIndex destination;
    internal TNodeIndex Destination => IsValid ? destination : throw ComponentInvalidException;

    internal TEdgeData Data;

    internal InternalEdge(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph,
        TEdgeIndex index, TNodeIndex origin, TNodeIndex destination, TEdgeData data) : base(graph)
    {
        this.origin = origin;
        this.destination = destination;
        this.index = index;
        Data = data;
    }
}