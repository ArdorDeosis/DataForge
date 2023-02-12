namespace DataForge.Graphs;

/// <summary>
/// An interface for a graph data structure.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{
  /// <summary>
  /// Removes the specified node from the graph.
  /// </summary>
  /// <param name="node">The node to remove from the graph.</param>
  /// <returns>True if the node was successfully removed, false otherwise.</returns>
  bool RemoveNode(INode<TNodeData, TEdgeData> node);

  /// <summary>
  /// Removes the specified edge from the graph.
  /// </summary>
  /// <param name="edge">The edge to remove from the graph.</param>
  /// <returns>True if the edge was successfully removed, false otherwise.</returns>
  bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);

  /// <summary>
  /// Removes all nodes from the graph that match the specified predicate.
  /// </summary>
  /// <param name="predicate">The predicate to match nodes against.</param>
  /// <returns>The number of nodes removed from the graph.</returns>
  int RemoveNodesWhere(Predicate<TNodeData> predicate);

  /// <summary>
  /// Removes all edges from the graph that match the specified predicate.
  /// </summary>
  /// <param name="predicate">The predicate to match edges against.</param>
  /// <returns>The number of edges removed from the graph.</returns>
  int RemoveEdgesWhere(Predicate<TEdgeData> predicate);

  /// <summary>
  /// Removes all nodes and edges from the graph.
  /// </summary>
  void Clear();

  /// <summary>
  /// Removes all edges from the graph.
  /// </summary>
  void ClearEdges();
}