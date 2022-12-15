namespace Graph;

public interface IEdgeIndexedNodeReadModule<TNodeData, TEdgeIndex, TEdgeData> :
  INodeReadModule<TNodeData, TEdgeData>
  where TEdgeIndex : notnull
{
  new IEnumerable<IEdgeIndexedNode<TNodeData, TEdgeIndex, TEdgeData>> Nodes { get; }
}