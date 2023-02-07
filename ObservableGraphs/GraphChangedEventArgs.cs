using DataForge.Graphs;
using JetBrains.Annotations;

namespace DataForge.ObservableGraphs;

[PublicAPI]
public sealed class GraphChangedEventArgs<TNodeData, TEdgeData> : EventArgs
{
  // TODO: remove builder methods and add default values to the properties instead
  internal GraphChangedEventArgs(){}
  internal GraphChangedEventArgs(
    IReadOnlyCollection<Node<TNodeData, TEdgeData>> addedNodes,
    IReadOnlyCollection<Node<TNodeData, TEdgeData>> removedNodes,
    IReadOnlyCollection<Edge<TNodeData, TEdgeData>> addedEdges,
    IReadOnlyCollection<Edge<TNodeData, TEdgeData>> removedEdges
  )
  {
    AddedNodes = addedNodes;
    RemovedNodes = removedNodes;
    AddedEdges = addedEdges;
    RemovedEdges = removedEdges;
  }

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> AddedNodes { get; init; }
  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> RemovedNodes { get; init; }
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> AddedEdges { get; init; }
  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> RemovedEdges { get; init; }

  internal static GraphChangedEventArgs<TNodeData, TEdgeData> NodesAdded(params Node<TNodeData, TEdgeData>[] nodes) =>
    new(
      nodes,
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      Array.Empty<Edge<TNodeData, TEdgeData>>(),
      Array.Empty<Edge<TNodeData, TEdgeData>>()
    );
  
  internal static GraphChangedEventArgs<TNodeData, TEdgeData> NodesRemoved(params Node<TNodeData, TEdgeData>[] nodes) =>
    new(
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      nodes,
      Array.Empty<Edge<TNodeData, TEdgeData>>(),
      Array.Empty<Edge<TNodeData, TEdgeData>>()
    );
  
  internal static GraphChangedEventArgs<TNodeData, TEdgeData> EdgesAdded(params Edge<TNodeData, TEdgeData>[] edges) =>
    new(
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      edges,
      Array.Empty<Edge<TNodeData, TEdgeData>>()
    );
  
  internal static GraphChangedEventArgs<TNodeData, TEdgeData> EdgesRemoved(params Edge<TNodeData, TEdgeData>[] edges) =>
    new(
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      Array.Empty<Edge<TNodeData, TEdgeData>>(),
      edges
    );
}