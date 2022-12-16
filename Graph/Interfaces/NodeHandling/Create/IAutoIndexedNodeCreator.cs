namespace Graph;

public interface IAutoIndexedNodeCreator<TNodeIndex, TNodeData>
  where TNodeIndex : notnull
{
  TNodeIndex AddNode(TNodeData data);
  bool TryAddNode(TNodeData data, out TNodeIndex index);
}