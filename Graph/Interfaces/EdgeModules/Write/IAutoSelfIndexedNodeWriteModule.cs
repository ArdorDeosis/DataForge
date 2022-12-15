namespace Graph;

public interface IAutoSelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData>
  where TEdgeIndex : notnull
{
  TEdgeIndex AddNode(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination, TEdgeData data);

  bool TryAddNode(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination, TEdgeData data,
    out TEdgeData index);
}