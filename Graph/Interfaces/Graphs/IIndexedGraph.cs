using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node);
  public bool RemoveNode(TIndex index);
  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge);

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination,
    TEdgeData data);

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge);
}