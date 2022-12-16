namespace Graph;

public interface IManuallyIndexedNodeCreator<TNodeIndex, TNodeData>
  where TNodeIndex : notnull
{
  void AddNode(TNodeIndex index, TNodeData data);

  bool TryAddNode(TNodeIndex index, TNodeData data);
}