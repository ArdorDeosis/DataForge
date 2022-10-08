using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// Options for the creation of a graph with a grid structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
[PublicAPI]
public sealed class GridGraphCreationOption<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOption<IReadOnlyList<int>, TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // TODO: These warnings should vanish when the required keyword comes

  /// <summary>
  /// Dimensional information about the grid including size, wrap and edge direction. Each element represents one
  /// dimension in the grid. 
  /// </summary>
  public /*required*/ IReadOnlyList<GridGraphDimensionInformation> DimensionInformation { get; init; }

#pragma warning restore CS8618
}