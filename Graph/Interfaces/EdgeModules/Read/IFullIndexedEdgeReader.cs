using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IFullIndexedEdgeReader<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IEdgeIndexedEdgeReader<TNodeData, TEdgeIndex, TEdgeData>,
  INodeIndexedEdgeReader<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  public new IEnumerable<ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Edges { get; }

  public new ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> this[TEdgeIndex index] { get; }
  public new ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetEdge(TEdgeIndex index);
  public new ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? GetEdgeOrNull(TEdgeIndex index);

  public bool TryGetEdge(TEdgeIndex index,
    [NotNullWhen(true)] out ISelfAndNodeIndexedEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}