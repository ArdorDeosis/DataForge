using JetBrains.Annotations;

namespace DataForge.Graphs.Observable;

/// <summary>
/// Data for the <see cref="IObservableGraph{TNodeData,TEdgeData}.GraphChanged" /> event of an
/// <see cref="IObservableGraph{TNodeData,TEdgeData}" />.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
[PublicAPI]
public interface IGraphChangedEventArgs<TNodeData, TEdgeData>
{
  /// <summary>
  /// The nodes added to the graph.
  /// </summary>
  public IReadOnlyCollection<INode<TNodeData, TEdgeData>> AddedNodes { get; }

  /// <summary>
  /// The nodes removed from the graph.
  /// </summary>
  public IReadOnlyCollection<INode<TNodeData, TEdgeData>> RemovedNodes { get; }

  /// <summary>
  /// The edges added to the graph.
  /// </summary>
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> AddedEdges { get; }

  /// <summary>
  /// The edges removed from the graph.
  /// </summary>
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> RemovedEdges { get; }
}