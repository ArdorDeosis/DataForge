using DataForge.Graphs;

namespace ObservableGraphs;

public interface IObservableUnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>> GraphChanged;
}