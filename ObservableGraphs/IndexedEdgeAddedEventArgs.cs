using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class IndexedEdgeAddedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  public IndexedEdgeAddedEventArgs(IndexedEdge<TIndex, TNodeData, TEdgeData> addedEdge)
  {
    AddedEdge = addedEdge;
  }
  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddedEdge {get;}
  
  public static implicit operator IndexedEdgeAddedEventArgs<TIndex, TNodeData, TEdgeData> (IndexedEdge<TIndex, TNodeData, TEdgeData> node) => new (node);
}