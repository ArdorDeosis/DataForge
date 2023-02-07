using DataForge.Graphs;

namespace DataForge.ObservableGraphs;

public interface IObservableUnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>> GraphChanged;
}