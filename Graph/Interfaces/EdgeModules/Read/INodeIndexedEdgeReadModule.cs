namespace Graph;

public interface INodeIndexedEdgeReadModule<TNodeIndex, TNodeData, TEdgeData> :
  IEdgeReadModule<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  new IEnumerable<INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData>> Edges { get; }
}