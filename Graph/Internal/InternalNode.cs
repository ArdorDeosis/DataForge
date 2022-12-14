namespace Graph;

internal sealed class
    InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> : GraphComponentHandle<TNodeIndex, TNodeData, TEdgeIndex,
        TEdgeData> where TNodeIndex : notnull where TEdgeIndex : notnull
{
    internal readonly TNodeIndex Index;
    internal TNodeData Data;

    internal InternalNode(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph, TNodeIndex index,
        TNodeData data) : base(graph)
    {
        Index = index;
        Data = data;
    }
}