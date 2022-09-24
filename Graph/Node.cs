using System.Diagnostics.CodeAnalysis;

namespace Graph;

/// <inheritdoc />
/// <summary>
/// The node of a graph.
/// </summary>
public sealed class Node<TNodeData, TEdgeData> : GraphComponent<TNodeData, TEdgeData>
{
  /// <summary>
  /// All edges coming to this node.
  /// </summary>
  internal readonly List<Edge<TNodeData, TEdgeData>> InternalIncomingEdgeList = new();

  /// <summary>
  /// All edges going away from this node.
  /// </summary>
  internal readonly List<Edge<TNodeData, TEdgeData>> InternalOutgoingEdgeList = new();

  internal Node(GraphBase<TNodeData, TEdgeData> graph, TNodeData data) : base(graph)
  {
    Data = data;
  }

  /// <summary>
  /// All edges connected to this node.
  /// </summary>
  [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
  public IEnumerable<Edge<TNodeData, TEdgeData>> Edges => Enumerable.Concat(IncomingEdges, OutgoingEdges);

  /// <summary>
  /// All edges coming to this node.
  /// </summary>
  public IEnumerable<Edge<TNodeData, TEdgeData>> IncomingEdges => InternalIncomingEdgeList;

  /// <summary>
  /// All edges going away from this node.
  /// </summary>
  public IEnumerable<Edge<TNodeData, TEdgeData>> OutgoingEdges => InternalOutgoingEdgeList;

  /// <summary>
  /// This node's data.
  /// </summary>
  public TNodeData Data { get; set; }

  /// <summary>
  /// All nodes from which edges go to this node.
  /// </summary>
  public IEnumerable<Node<TNodeData, TEdgeData>> Predecessors => IncomingEdges.Select(edge => edge.Start).ToHashSet();

  /// <summary>
  /// All nodes to which edges go from this node.
  /// </summary>
  public IEnumerable<Node<TNodeData, TEdgeData>> Successors => OutgoingEdges.Select(edge => edge.End).ToHashSet();

  /// <summary>
  /// All nodes connected with an edge to this node.
  /// </summary>
  [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
  public IEnumerable<Node<TNodeData, TEdgeData>> Neighbours =>
    Enumerable.Concat(
      IncomingEdges.Select(edge => edge.Start),
      OutgoingEdges.Select(edge => edge.End)
    ).ToHashSet();
}