namespace Graph;

public interface INode<TNodeData, TEdgeData>
{
    public IGraph<TNodeData, TEdgeData> Graph { get; }
    public TNodeData Data { get; set; }
    public bool IsValid { get; }
}

public interface IIndexedNode<TNodeIndex ,TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
{
    public TNodeIndex Index { get; }
}