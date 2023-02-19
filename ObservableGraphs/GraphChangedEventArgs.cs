using JetBrains.Annotations;

namespace DataForge.Graphs.Observable;

/// <summary>
/// Data for the <see cref="IObservableUnindexedGraph{TNodeData,TEdgeData}.GraphChanged" /> event of an
/// <see cref="IObservableUnindexedGraph{TNodeData,TEdgeData}" />.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
[PublicAPI]
public sealed class GraphChangedEventArgs<TNodeData, TEdgeData> : EventArgs,
  IGraphChangedEventArgs<TNodeData, TEdgeData>
{
  internal GraphChangedEventArgs()
  {
    AddedNodes = Array.Empty<Node<TNodeData, TEdgeData>>();
    RemovedNodes = Array.Empty<Node<TNodeData, TEdgeData>>();
    AddedEdges = Array.Empty<Edge<TNodeData, TEdgeData>>();
    RemovedEdges = Array.Empty<Edge<TNodeData, TEdgeData>>();
  }

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.AddedNodes" />
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> AddedNodes { get; init; }

  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.AddedNodes => AddedNodes;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.RemovedNodes" />
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> RemovedNodes { get; init; }
  
  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.RemovedNodes => RemovedNodes;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.AddedEdges" />
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> AddedEdges { get; init; }
  
  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.AddedEdges => AddedEdges;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.RemovedEdges" />
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> RemovedEdges { get; init; }

  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.RemovedEdges => RemovedEdges;
}