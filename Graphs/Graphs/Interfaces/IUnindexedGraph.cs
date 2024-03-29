﻿using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

/// <summary>
/// An interface for an unindexed graph data structure.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IUnindexedGraph<TNodeData, TEdgeData> : IReadOnlyUnindexedGraph<TNodeData, TEdgeData>,
  IGraph<TNodeData, TEdgeData>
{
  /// <summary>
  /// Adds a new node to the graph with the specified data.
  /// </summary>
  /// <param name="data">The data to store in the new node.</param>
  /// <returns>The newly added node.</returns>
  INode<TNodeData, TEdgeData> AddNode(TNodeData data);

  /// <summary>
  /// Adds multiple new nodes to the graph with the specified data.
  /// </summary>
  /// <param name="data">The data to store in the new nodes.</param>
  /// <returns>An enumerable containing the newly added nodes.</returns>
  IEnumerable<INode<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data) => data.Select(AddNode).ToArray();

  /// <inheritdoc cref="AddNodes(System.Collections.Generic.IEnumerable{TNodeData})" />
  IEnumerable<INode<TNodeData, TEdgeData>> AddNodes(params TNodeData[] data) => data.Select(AddNode).ToArray();

  /// <summary>
  /// Adds a new edge to the graph connecting the specified origin and destination nodes with the specified data.
  /// </summary>
  /// <param name="origin">The origin node of the new edge.</param>
  /// <param name="destination">The destination node of the new edge.</param>
  /// <param name="data">The data to store in the new edge.</param>
  /// <exception cref="ArgumentException">If the origin or destination node are not in the graph.</exception>
  /// <returns>The newly added edge.</returns>
  IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination,
    TEdgeData data);

  /// <summary>
  /// Attempts to add a new edge to the graph connecting the specified origin and destination nodes with the specified
  /// data.
  /// </summary>
  /// <param name="origin">The origin node of the new edge.</param>
  /// <param name="destination">The destination node of the new edge.</param>
  /// <param name="data">The data to store in the new edge.</param>
  /// <param name="edge">The newly added edge, or null if the edge could not be added.</param>
  /// <returns>True if the edge was successfully added, false otherwise.</returns>
  bool TryAddEdge(INode<TNodeData, TEdgeData> origin, INode<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out IEdge<TNodeData, TEdgeData>? edge)
  {
    edge = Contains(origin) && Contains(destination) ? AddEdge(origin, destination, data) : null;
    return edge is not null;
  }
}