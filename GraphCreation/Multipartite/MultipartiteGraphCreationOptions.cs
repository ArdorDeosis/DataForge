using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for a multipartite graph.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
[PublicAPI]
public sealed class MultipartiteGraphCreationOptions<TNodeData, TEdgeData>
  : UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
  /// <summary>
  /// A collection of collections of node data. Edges are potentially created between all nodes except for nodes of the
  /// same collection.
  /// </summary>
  public required IEnumerable<IEnumerable<TNodeData>> NodeDataSets { get; init; }


  /// <summary>
  /// Function to determine whether an edge should be created between two nodes.
  /// <code>bool ShouldCreateEdge(TNodeData startNodeData, TNodeData endNodeData, EdgeDirection edgeDirection)</code>
  /// Expects the node data of the start and end node and an <see cref="EdgeDirection" /> and returns whether an edge
  /// should be created or not. This function is only ever called with
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> or
  /// <see cref="EdgeDirection.Backward">EdgeDirection.Backward</see><br />
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from nodes of a collection earlier in
  /// <see cref="NodeDataSets" /> to nodes of a set later in <see cref="NodeDataSets" />.
  /// </summary>
  /// <remarks>Default value is to create all <see cref="EdgeDirection.Forward" /> running edges.</remarks>
  public Func<TNodeData, TNodeData, EdgeDirection, bool> ShouldCreateEdge { get; init; } = (_, _, direction) =>
    direction == EdgeDirection.Forward;
}