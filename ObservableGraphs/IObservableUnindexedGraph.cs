using DataForge.Graphs;

namespace ObservableGraphs;

public interface IObservableUnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  event EventHandler<NodeAddedEventArgs<TNodeData, TEdgeData>> NodeAdded;
  event EventHandler<NodeRemovedEventArgs<TNodeData, TEdgeData>> NodeRemoved;
  event EventHandler<EdgeAddedEventArgs<TNodeData, TEdgeData>> EdgeAdded;
  event EventHandler<EdgeRemovedEventArgs<TNodeData, TEdgeData>> EdgeRemoved;
}