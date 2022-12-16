namespace Graph;

public interface IEdgeIndexedNodeReader<TNodeData, TEdgeIndex, TEdgeData> :
  INodeReader<TNodeData, TEdgeData>
  where TEdgeIndex : notnull
{
  new IEnumerable<IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData>> Nodes { get; }
}