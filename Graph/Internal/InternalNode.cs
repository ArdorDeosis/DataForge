namespace Graph;

internal sealed class
    InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex,
        TEdgeData> where TNodeIndex : notnull where TEdgeIndex : notnull
{
    private readonly TNodeIndex index;
    internal TNodeIndex Index => IsValid ? index : throw ComponentInvalidException;
    internal TNodeData Data;

    internal InternalNode(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph, TNodeIndex index,
        TNodeData data) : base(graph)
    {
        this.index = index;
        Data = data;
    }
}