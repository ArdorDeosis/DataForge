namespace DataForge.Graphs;

public interface IIndexedEdge<TIndex, TNodeData, TEdgeData> : IEdge<TNodeData, TEdgeData> where TIndex : notnull
{
  new IndexedNode<TIndex, TNodeData, TEdgeData> Origin { get; }
  new IndexedNode<TIndex, TNodeData, TEdgeData> Destination { get; }
}