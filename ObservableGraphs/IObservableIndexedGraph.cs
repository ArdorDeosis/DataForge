using DataForge.Graphs;

namespace ObservableGraphs;

public interface IObservableIndexedGraph<TIndex, TNodeData, TEdgeData> : IIndexedGraph<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>> GraphChanged;
}