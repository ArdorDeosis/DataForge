using JetBrains.Annotations;

namespace DataForge.Graphs.Observable;

/// <summary>
/// Data for the <see cref="IObservableUnindexedGraph{TNodeData,TEdgeData}.GraphChanged"/> event of an
/// <see cref="IObservableUnindexedGraph{TNodeData,TEdgeData}"/>. 
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
[PublicAPI]
public sealed class GraphChangedEventArgs<TNodeData, TEdgeData> : EventArgs
{
  internal GraphChangedEventArgs()
  {
    AddedNodes =  Array.Empty<Node<TNodeData, TEdgeData>>();
    RemovedNodes =  Array.Empty<Node<TNodeData, TEdgeData>>();
    AddedEdges =  Array.Empty<Edge<TNodeData, TEdgeData>>();
    RemovedEdges =  Array.Empty<Edge<TNodeData, TEdgeData>>();
  }

  /// <summary>
  /// The nodes added to the graph.
  /// </summary>
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> AddedNodes { get; init; }
  
  /// <summary>
  /// The nodes removed from the graph.
  /// </summary>
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> RemovedNodes { get; init; }
  
  /// <summary>
  /// The edges added to the graph.
  /// </summary>
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> AddedEdges { get; init; }
  
  /// <summary>
  /// The edges removed from the graph.
  /// </summary>
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> RemovedEdges { get; init; }
}