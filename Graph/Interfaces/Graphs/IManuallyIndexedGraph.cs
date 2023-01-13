using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data);

  public bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);
}