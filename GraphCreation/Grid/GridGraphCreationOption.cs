using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// Options for the creation of a graph with a grid structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
[PublicAPI]
public class GridGraphCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // TODO: I'd hope these warnings vanish with the required keyword

  /// <summary>
  /// Dimensional information about the grid including size, wrap and edge direction. Each element represents one
  /// dimension in the grid. 
  /// </summary>
  public /*required*/ IReadOnlyList<GridGraphDimensionInformation> DimensionInformation { get; init; }

  /// <summary>
  /// Function to create the node data.
  /// </summary>
  public /*required*/ Func<IReadOnlyList<int>, TNodeData> CreateNodeData { get; init; }

  /// <summary>
  /// Function to create the edge data.
  /// </summary>
  public /*required*/ Func<EdgeDefinition<IReadOnlyList<int>, TNodeData>, TEdgeData> CreateEdgeData { get; init; }

#pragma warning restore CS8618
}