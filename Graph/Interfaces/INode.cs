namespace Graph;

public interface INode<TNodeData, TEdgeData> : IGraphComponent<TNodeData, TEdgeData>
{
    public TNodeData Data { get; set; }
}