using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

/// <summary>
/// An interface for a read-only indexed graph data structure.
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IReadOnlyGraph<TNodeData, TEdgeData>
  where TIndex : notnull
{
  /// <summary>
  /// Gets a collection of all the indexed nodes in the graph.
  /// </summary>
  new IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> Nodes { get; }

  /// <summary>
  /// Gets a collection of all the indexed edges in the graph.
  /// </summary>
  new IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> Edges { get; }

  /// <summary>
  /// Gets a collection of all the indices used to identify nodes in the graph.
  /// </summary>
  IReadOnlyCollection<TIndex> Indices { get; }

  /// <summary>
  /// Gets the indexed node at the specified index.
  /// </summary>
  /// <param name="index">The index of the node to retrieve.</param>
  /// <returns>The indexed node at the specified index.</returns>
  IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] { get; }

  /// <summary>
  /// Gets the indexed node at the specified index.
  /// </summary>
  /// <param name="index">The index of the node to retrieve.</param>
  /// <returns>The indexed node at the specified index.</returns>
  IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index);

  /// <summary>
  /// Gets the indexed node at the specified index, or null if no such node exists.
  /// </summary>
  /// <param name="index">The index of the node to retrieve.</param>
  /// <returns>The indexed node at the specified index, or null if no such node exists.</returns>
  IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index);

  /// <summary>
  /// Attempts to retrieve the indexed node at the specified index.
  /// </summary>
  /// <param name="index">The index of the node to retrieve.</param>
  /// <param name="node">The indexed node at the specified index, or null if no such node exists.</param>
  /// <returns>True if a node was found at the specified index, false otherwise.</returns>
  bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);

  /// <summary>
  /// Determines whether the graph contains a specific index.
  /// </summary>
  /// <param name="index">The index to locate in the graph.</param>
  /// <returns>True if the graph contains the specified index, false otherwise.</returns>
  public bool Contains(TIndex index);
}