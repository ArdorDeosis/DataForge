namespace DataForge.Graphs;

/// <summary>
/// An interface for a read-only graph data structure.
/// </summary>
/// <typeparam name="TNodeData">The type of data stored in the graph's nodes.</typeparam>
/// <typeparam name="TEdgeData">The type of data stored in the graph's edges.</typeparam>
public interface IReadOnlyGraph<TNodeData, TEdgeData>
{
  /// <summary>
  /// Gets a collection of all the nodes in the graph.
  /// </summary>
  IReadOnlyCollection<INode<TNodeData, TEdgeData>> Nodes { get; }

  /// <summary>
  /// Gets a collection of all the edges in the graph.
  /// </summary>
  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get; }

  /// <summary>
  /// Determines whether the graph contains a specific node.
  /// </summary>
  /// <param name="node">The node to locate in the graph.</param>
  /// <returns>True if the graph contains the specified node, false otherwise.</returns>
  bool Contains(INode<TNodeData, TEdgeData> node);

  /// <summary>
  /// Determines whether the graph contains a specific edge.
  /// </summary>
  /// <param name="edge">The edge to locate in the graph.</param>
  /// <returns>True if the graph contains the specified edge, false otherwise.</returns>
  bool Contains(IEdge<TNodeData, TEdgeData> edge);

  /// <summary>
  /// Gets the number of nodes in the graph.
  /// </summary>
  int Order { get; }

  /// <summary>
  /// Gets the number of edges in the graph.
  /// </summary>
  int Size { get; }
}