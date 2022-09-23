using System.Diagnostics.CodeAnalysis;

namespace Graph;

public sealed class Node<TNodeData, TEdgeData> : GraphComponent<TNodeData, TEdgeData>
{
  internal readonly List<Edge<TNodeData, TEdgeData>> InternalIncomingEdgeList = new();
  internal readonly List<Edge<TNodeData, TEdgeData>> InternalOutgoingEdgeList = new();

  internal Node(GraphBase<TNodeData, TEdgeData> graph, TNodeData data) : base(graph)
  {
    Data = data;
  }

  [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
  public IEnumerable<Edge<TNodeData, TEdgeData>> Edges => Enumerable.Concat(InternalIncomingEdges, InternalOutgoingEdges);

  public IEnumerable<Edge<TNodeData, TEdgeData>> InternalIncomingEdges => InternalIncomingEdgeList;

  public IEnumerable<Edge<TNodeData, TEdgeData>> InternalOutgoingEdges => InternalOutgoingEdgeList;

  public TNodeData Data { get; set; }

  public IEnumerable<Node<TNodeData, TEdgeData>> Predecessors => InternalIncomingEdges.Select(edge => edge.Start).ToHashSet();
  public IEnumerable<Node<TNodeData, TEdgeData>> Successors => InternalOutgoingEdges.Select(edge => edge.End).ToHashSet();

  [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
  public IEnumerable<Node<TNodeData, TEdgeData>> Neighbours =>
    Enumerable.Concat(
      InternalIncomingEdges.Select(edge => edge.Start),
      InternalOutgoingEdges.Select(edge => edge.End)
    ).ToHashSet();
}