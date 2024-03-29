﻿namespace DataForge.Graphs;

/// <summary>
/// An interface for a read-only unindexed graph data structure.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IReadOnlyUnindexedGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{
  /// <summary>
  /// Gets a collection of all the unindexed nodes in the graph.
  /// </summary>
  new IReadOnlyCollection<INode<TNodeData, TEdgeData>> Nodes { get; }

  /// <summary>
  /// Gets a collection of all the unindexed edges in the graph.
  /// </summary>
  new IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get; }
}