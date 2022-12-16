using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedEdgeRemover<TNodeData, TEdgeIndex, TEdgeData> :
  IEdgeRemover<TNodeData, TEdgeData>
  where TEdgeIndex : notnull
{
  bool RemoveEdge(TEdgeIndex index);
  bool RemoveEdge(TEdgeIndex index, [NotNullWhen(true)] out ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData>? node);
}