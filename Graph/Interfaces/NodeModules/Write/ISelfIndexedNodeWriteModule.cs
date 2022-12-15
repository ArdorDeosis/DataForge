using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface ISelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData> :
  INodeWriteModule<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  bool RemoveNode(TNodeIndex index);
  bool RemoveNode(TNodeIndex index, [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);
}