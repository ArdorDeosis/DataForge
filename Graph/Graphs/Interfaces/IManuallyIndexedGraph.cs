using System.Diagnostics.CodeAnalysis;

namespace Graph;

/// <summary>
/// An interface for a manually indexed graph data structure.
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  /// <summary>
  /// Adds a new indexed node to the graph with the specified index and data.
  /// </summary>
  /// <param name="index">The index to assign to the new node.</param>
  /// <param name="data">The data to store in the new node.</param>
  /// <returns>The newly added indexed node.</returns>
  IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data);

  /// <summary>
  /// Attempts to add a new indexed node to the graph with the specified index and data.
  /// </summary>
  /// <param name="index">The index to assign to the new node.</param>
  /// <param name="data">The data to store in the new node.</param>
  /// <param name="node">The newly added indexed node, or null if a node with the same index already exists.</param>
  /// <returns>True if the node was successfully added, false otherwise.</returns>
  bool TryAddNode(TIndex index, TNodeData data,
    [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node);
}