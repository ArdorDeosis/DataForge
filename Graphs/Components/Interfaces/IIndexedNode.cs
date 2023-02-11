namespace DataForge.Graphs;

public interface IIndexedNode<TIndex, TNodeData, TEdgeData> : INode<TNodeData, TEdgeData> where TIndex : notnull
{
  TIndex Index { get; }
  new IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges { get; }
  new IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> IncomingEdges { get; }
  new IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> OutgoingEdges { get; }
  new IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Neighbours { get; }
  bool TryGetIndex(out TIndex index);
}