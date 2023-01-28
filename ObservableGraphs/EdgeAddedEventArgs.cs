using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class EdgeAddedEventArgs<TNodeData, TEdgeData> : EventArgs {
  public EdgeAddedEventArgs(Edge<TNodeData, TEdgeData> addedEdge)
  {
    AddedEdge = addedEdge;
  }
  public Edge<TNodeData, TEdgeData> AddedEdge {get;}
  
  public static implicit operator EdgeAddedEventArgs<TNodeData, TEdgeData> (Edge<TNodeData, TEdgeData> node) => new (node);
}