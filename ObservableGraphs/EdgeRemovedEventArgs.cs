using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class EdgeRemovedEventArgs<TNodeData, TEdgeData> : EventArgs {
  public EdgeRemovedEventArgs(Edge<TNodeData, TEdgeData> removedEdge)
  {
    RemovedEdge = removedEdge;
  }
  public Edge<TNodeData, TEdgeData> RemovedEdge {get;}
  
  public static implicit operator EdgeRemovedEventArgs<TNodeData, TEdgeData> (Edge<TNodeData, TEdgeData> node) => new (node);
}