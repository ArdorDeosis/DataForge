namespace Graph;

public interface IEdgeReader<TNodeData, TEdgeData>
{
  IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
  bool Contains(IEdge<TNodeData, TEdgeData> edge);
}