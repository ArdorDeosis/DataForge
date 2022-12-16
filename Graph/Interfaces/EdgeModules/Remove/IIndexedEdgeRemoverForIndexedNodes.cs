using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedEdgeRemoverForIndexedNodes<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IIndexedEdgeRemover<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  bool RemoveEdge(TEdgeIndex index,
    [NotNullWhen(true)] out ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}