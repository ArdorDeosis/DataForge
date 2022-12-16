namespace Graph;

public interface INodeIndexedEdgeReader<TNodeIndex, TNodeData, TEdgeData> :
  IEdgeReader<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  new IEnumerable<INodeIndexedEdge<TNodeIndex, TNodeData, TEdgeData>> Edges { get; }
}