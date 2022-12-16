using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedNodeRemover<TNodeIndex, TNodeData, TEdgeData> :
  INodeRemover<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  bool RemoveNode(TNodeIndex index);
  bool RemoveNode(TNodeIndex index, [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);
}