namespace Graph;

public interface IGraphComponent<TNodeData, TEdgeData>
{
    IGraph<TNodeData, TEdgeData> Graph { get; }
    bool IsValid { get; }
}