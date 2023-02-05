using DataForge.Graphs;
using JetBrains.Annotations;

namespace DataForge.ObservableGraphs;

[PublicAPI]
public sealed class IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  // TODO: remove builder methods and add default values to the properties instead
  internal IndexedGraphChangedEventArgs(){}
  
  internal IndexedGraphChangedEventArgs(
    IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> addedNodes,
    IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> removedNodes,
    IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> addedEdges,
    IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> removedEdges
  )
  {
    AddedNodes = addedNodes;
    RemovedNodes = removedNodes;
    AddedEdges = addedEdges;
    RemovedEdges = removedEdges;
  }

  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> AddedNodes { get; init; }
  public IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> RemovedNodes { get; init; }
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> AddedEdges { get; init; }
  public IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> RemovedEdges { get; init; }

  internal static IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> NodesAdded(
    params IndexedNode<TIndex, TNodeData, TEdgeData>[] nodes) =>
    new(
      nodes,
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>()
    );

  internal static IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> NodesRemoved(
    params IndexedNode<TIndex, TNodeData, TEdgeData>[] nodes) =>
    new(
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      nodes,
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>()
    );

  internal static IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> EdgesAdded(
    params IndexedEdge<TIndex, TNodeData, TEdgeData>[] edges) =>
    new(
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      edges,
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>()
    );

  internal static IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> EdgesRemoved(
    params IndexedEdge<TIndex, TNodeData, TEdgeData>[] edges) =>
    new(
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>(),
      edges
    );
}