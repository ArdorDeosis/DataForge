namespace DataForge.Graphs.Observable;

public interface IObservableUnindexedGraph<TNodeData, TEdgeData> : IUnindexedGraph<TNodeData, TEdgeData>,
  IObservableGraph<TNodeData, TEdgeData>
{
  new event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>> GraphChanged;
}