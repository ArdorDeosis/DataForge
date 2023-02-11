namespace DataForge.Graphs.Observable;

public interface IObservableUnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>
{
  event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>> GraphChanged;
}