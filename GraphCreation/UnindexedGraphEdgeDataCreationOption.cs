using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for edge data in a non-indexed graph.
/// </summary>
/// <typeparam name="TNodeData">Type of the node data of the graph to be created.</typeparam>
/// <typeparam name="TEdgeData">Type of the edge data of the graph to be created.</typeparam>
/// [PublicAPI]
public abstract class UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
  /// <summary>
  /// Function to create edge data based on the data of the nodes it is connecting.
  /// <code>TEdgeData CreateEdgeData(TNodeData startNodeData, TNodeData endNodeData)</code>
  /// It expects the node data of the start node as first and the node data of the end node as second parameter and
  /// returns the edge data.
  /// </summary>
  public required Func<TNodeData, TNodeData, TEdgeData> CreateEdgeData { get; init; }
}