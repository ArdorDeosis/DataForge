using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class IndexedEdgeRemovedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  public IndexedEdgeRemovedEventArgs(IndexedEdge<TIndex, TNodeData, TEdgeData> removedEdge)
  {
    RemovedEdge = removedEdge;
  }
  public IndexedEdge<TIndex, TNodeData, TEdgeData> RemovedEdge {get;}
  
  public static implicit operator IndexedEdgeRemovedEventArgs<TIndex, TNodeData, TEdgeData> (IndexedEdge<TIndex, TNodeData, TEdgeData> node) => new (node);
}