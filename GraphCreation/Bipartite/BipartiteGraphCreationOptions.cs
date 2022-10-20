﻿using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// Creation options for a bipartite graph.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
[PublicAPI]
public sealed class BipartiteGraphCreationOptions<TNodeData, TEdgeData>
  : UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // this should be unnecessary once the required keyword is in use
  /// <summary>
  /// First set of node data to create nodes from.
  /// </summary>
  public /*required*/ IEnumerable<TNodeData> NodeDataSetA { get; init; }

  /// <summary>
  /// Second set of node data to create nodes from.
  /// </summary>
  public /*required*/ IEnumerable<TNodeData> NodeDataSetB { get; init; }
#pragma warning restore CS8618

  /// <summary>
  /// Function to determine whether an edge should be created between two nodes.
  /// <code>bool ShouldCreateEdge(TNodeData startNodeData, TNodeData endNodeData, EdgeDirection edgeDirection)</code>
  /// Expects the node data of the start and end node and an <see cref="EdgeDirection"/> and returns whether an edge
  /// should be created or not. This function is only ever called with
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> or
  /// <see cref="EdgeDirection.Backward">EdgeDirection.Backward</see><br/>
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from nodes of <see cref="NodeDataSetA"/> to nodes
  /// of <see cref="NodeDataSetB"/>.
  /// </summary>
  /// <remarks>Default value is to create all <see cref="EdgeDirection.Forward"/> running edges.</remarks>
  public Func<TNodeData, TNodeData, EdgeDirection, bool> ShouldCreateEdge { get; init; } = (_, _, direction) =>
    direction == EdgeDirection.Forward;
}