using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IEdgeIndexedEdgeReader<TNodeData, TEdgeIndex, TEdgeData> :
  IEdgeReader<TNodeData, TEdgeData>
  where TEdgeIndex : notnull
{
  public new IEnumerable<ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData>> Edges { get; }
  public IEnumerable<TEdgeIndex> EdgeIndices { get; }

  public ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData> this[TEdgeIndex index] { get; }
  public ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData> GetEdge(TEdgeIndex index);
  public ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData>? GetEdgeOrNull(TEdgeIndex index);

  public bool TryGetEdge(TEdgeIndex index,
    [NotNullWhen(true)] out ISelfIndexedEdge<TNodeData, TEdgeIndex, TEdgeData>? node);

  public bool TryGetEdgeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TEdgeIndex? index);
  public bool Contains(TEdgeIndex index);
  public bool ContainsEdge(TEdgeIndex index);
}