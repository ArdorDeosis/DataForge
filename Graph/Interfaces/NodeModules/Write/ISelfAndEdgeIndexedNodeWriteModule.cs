using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface ISelfAndEdgeIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  bool RemoveNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}