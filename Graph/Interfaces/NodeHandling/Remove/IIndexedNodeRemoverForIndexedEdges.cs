using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedNodeRemoverForIndexedEdges<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IIndexedNodeRemover<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  bool RemoveNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}