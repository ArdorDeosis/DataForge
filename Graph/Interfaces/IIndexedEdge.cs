namespace Graph;

public interface IIndexedEdge<TNodeIndex, TNodeData, TEdgeData> : IGraphComponent<TNodeData, TEdgeData>
{
    TEdgeData Data { get; set; }
    IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Origin { get; }
    IIndexedNode<TNodeIndex, TNodeData, TEdgeData> Destination { get; }
}