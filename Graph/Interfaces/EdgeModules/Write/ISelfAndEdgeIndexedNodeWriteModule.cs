using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface ISelfAndNodeIndexedEdgeWriteModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  ISelfIndexedEdgeWriteModule<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  bool RemoveEdge(TEdgeIndex index,
    [NotNullWhen(true)] out ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}