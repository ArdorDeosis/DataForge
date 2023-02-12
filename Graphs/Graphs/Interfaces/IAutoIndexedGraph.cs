using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs;

/// <summary>
/// An interface for an automatically indexed graph data structure.
/// </summary>
/// <typeparam name="TIndex">The type of index used to identify nodes in the graph.</typeparam>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IAutoIndexedGraph<TIndex, TNodeData, TEdgeData> :
  IIndexedGraph<TIndex, TNodeData, TEdgeData>
  where TIndex : notnull
{
  /// <summary>
  /// Adds a new indexed node to the graph with the specified data, and assigns the next available index to the new node.
  /// </summary>
  /// <param name="data">The data to store in the new node.</param>
  /// <returns>The newly added indexed node.</returns>
  IIndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TNodeData data);

  /// <summary>
  /// Attempts to add a new indexed node to the graph with the specified data, and assigns the next available index to the
  /// new node.
  /// </summary>
  /// <param name="data">The data to store in the new node.</param>
  /// <param name="node">The newly added indexed node, or null if the node could not be added.</param>
  /// <returns>True if the node was successfully added, false otherwise.</returns>
  bool TryAddNode(TNodeData data, [NotNullWhen(true)] out IIndexedNode<TIndex, TNodeData, TEdgeData>? node);
}