using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IManualSelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData> :
  ISelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> AddNode(TNodeIndex index, TNodeData data);

  bool TryAddNode(TNodeIndex index, TNodeData data,
    [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);
}