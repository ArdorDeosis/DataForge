using DataForge.Graphs;

namespace ObservableGraphs;

public interface IObservableIndexedGraph<TIndex, TNodeData, TEdgeData> : IIndexedGraph<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
  event EventHandler<IndexedNodeAddedEventArgs<TIndex, TNodeData, TEdgeData>> IndexedNodeAdded;
  event EventHandler<IndexedNodeRemovedEventArgs<TIndex, TNodeData, TEdgeData>> IndexedNodeRemoved;
  event EventHandler<IndexedEdgeAddedEventArgs<TIndex, TNodeData, TEdgeData>> IndexedEdgeAdded;
  event EventHandler<IndexedEdgeRemovedEventArgs<TIndex, TNodeData, TEdgeData>> IndexedEdgeRemoved;
}