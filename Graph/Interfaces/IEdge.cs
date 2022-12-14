namespace Graph;

public interface IEdge<TNodeData, TEdgeData>
{
    IGraph<TNodeData, TEdgeData> Graph { get; }
    TEdgeData Data { get; set; }
    bool IsValid { get; }
    void Invalidate();
}