using JetBrains.Annotations;

namespace DataForge.Graphs.Observable;

/// <summary>
/// Data for the <see cref="IObservableIndexedGraph{TIndex,TNodeData,TEdgeData}.GraphChanged" /> event of an
/// <see cref="IObservableIndexedGraph{TIndex,TNodeData,TEdgeData}" />.
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
[PublicAPI]
public sealed class IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs,
  IGraphChangedEventArgs<TNodeData, TEdgeData> where TIndex : notnull
{
  internal IndexedGraphChangedEventArgs(
  )
  {
    AddedNodes = Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>();
    RemovedNodes = Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>();
    AddedEdges = Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>();
    RemovedEdges = Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>();
  }

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.AddedNodes" />
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> AddedNodes { get; init; }

  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.AddedNodes => AddedNodes;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.RemovedNodes" />
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> RemovedNodes { get; init; }
  
  /// <inheritdoc />
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.RemovedNodes => RemovedNodes;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.AddedEdges" />
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> AddedEdges { get; init; }
  
  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.AddedEdges => AddedEdges;

  /// <inheritdoc cref="IGraphChangedEventArgs{TNodeData,TEdgeData}.RemovedEdges" />
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> RemovedEdges { get; init; }

  /// <inheritdoc />
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IGraphChangedEventArgs<TNodeData, TEdgeData>.RemovedEdges => RemovedEdges;
}