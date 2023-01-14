using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>
  where TIndex : notnull
{
  public new IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes { get; }
  public new IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges { get; }
  public IReadOnlyCollection<TIndex> Indices { get; }

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] { get; }
  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index);
  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index);
  public bool TryGetNode(TIndex index,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);
  
  public bool Contains(TIndex index);
}