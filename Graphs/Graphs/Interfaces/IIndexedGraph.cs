using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

/// <summary>
/// An interface for an indexed graph data structure.
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  /// <summary>
  /// Removes the specified indexed node from the graph.
  /// </summary>
  /// <param name="node">The indexed node to remove from the graph.</param>
  /// <returns>True if the indexed node was successfully removed, false otherwise.</returns>
  bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node);

  /// <summary>
  /// Removes the indexed node with the specified index from the graph.
  /// </summary>
  /// <param name="index">The index of the indexed node to remove from the graph.</param>
  /// <returns>True if the indexed node was successfully removed, false otherwise.</returns>
  bool RemoveNode(TIndex index);

  /// <summary>
  /// Attempts to remove the indexed node with the specified index from the graph.
  /// </summary>
  /// <param name="index">The index of the indexed node to remove from the graph.</param>
  /// <param name="node">The removed indexed node, or null if no such node was found.</param>
  /// <returns>True if the indexed node was successfully removed, false otherwise.</returns>
  bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);

  /// <summary>
  /// Adds a new edge to the graph connecting the indexed nodes with the specified indices, and stores the specified data in the new edge.
  /// </summary>
  /// <param name="origin">The index of the origin node of the new edge.</param>
  /// <param name="destination">The index of the destination node of the new edge.</param>
  /// <param name="data">The data to store in the new edge.</param>
  /// <returns>The newly added indexed edge.</returns>
  IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination,
    TEdgeData data);

  /// <summary>
  /// Attempts to add a new edge to the graph connecting the indexed nodes with the specified indices, and stores the
  /// specified data in the new edge.
  /// </summary>
  /// <param name="origin">The index of the origin node of the new edge.</param>
  /// <param name="destination">The index of the destination node of the new edge.</param>
  /// <param name="data">The data to store in the new edge.</param>
  /// <param name="edge">The newly added indexed edge, or null if the edge could not be added.</param>
  /// <returns>True if the edge was successfully added, false otherwise.</returns>
  bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data,
    [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge);

  /// <summary>
  /// Removes the specified indexed edge from the graph.
  /// </summary>
  /// <param name="edge">The indexed edge to remove from the graph.</param>
  /// <returns>True if the indexed edge was successfully removed, false otherwise.</returns>
  bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge);
}