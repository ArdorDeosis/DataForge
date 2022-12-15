namespace Graph;

public interface IEdgeReadModule<TNodeData, TEdgeData>
{
  IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
  bool Contains(IEdge<TNodeData, TEdgeData> edge);
}