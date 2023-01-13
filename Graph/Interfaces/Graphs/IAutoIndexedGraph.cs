using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IAutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data);

  public bool TryAddNode(TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);

}