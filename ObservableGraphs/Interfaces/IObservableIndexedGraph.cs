namespace DataForge.Graphs.Observable;

public interface IObservableIndexedGraph<TIndex, TNodeData, TEdgeData> : IIndexedGraph<TIndex, TNodeData, TEdgeData>, 
  IObservableGraph<TNodeData, TEdgeData>
  where TIndex : notnull
{
  new event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>> GraphChanged;
}