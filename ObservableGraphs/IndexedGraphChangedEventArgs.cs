using DataForge.Graphs;
using JetBrains.Annotations;

namespace DataForge.ObservableGraphs;

/// <summary>
/// Data for the <see cref="IObservableIndexedGraph{TIndex,TNodeData,TEdgeData}.GraphChanged"/> event of an
/// <see cref="IObservableIndexedGraph{TIndex,TNodeData,TEdgeData}"/>. 
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
[PublicAPI]
public sealed class IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  internal IndexedGraphChangedEventArgs(
  )
  {
    AddedNodes = Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>();
    RemovedNodes = Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>();
    AddedEdges = Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>();
    RemovedEdges = Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>();
  }
  
  /// <summary>
  /// The nodes added to the graph.
  /// </summary>
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> AddedNodes { get; init; }
  
  /// <summary>
  /// The nodes removed from the graph.
  /// </summary>
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> RemovedNodes { get; init; }
  
  /// <summary>
  /// The edges added to the graph.
  /// </summary>
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> AddedEdges { get; init; }
  
  /// <summary>
  /// The edges removed from the graph.
  /// </summary>
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> RemovedEdges { get; init; }
}