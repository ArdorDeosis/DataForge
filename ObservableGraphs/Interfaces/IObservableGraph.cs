namespace DataForge.Graphs.Observable;

public interface IObservableGraph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
{
  event EventHandler<IGraphChangedEventArgs<TNodeData, TEdgeData>> GraphChanged;
}