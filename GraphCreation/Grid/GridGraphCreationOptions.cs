﻿using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Options for the creation of a graph with a grid structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
[PublicAPI]
public sealed class GridGraphCreationOptions<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOptions<IReadOnlyList<int>, TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // these warnings should vanish when the required keyword comes

  /// <summary>
  /// Dimensional information about the grid including size, wrap and edge direction. Each element represents one
  /// dimension in the grid. 
  /// </summary>
  public /*required*/ IReadOnlyList<GridGraphDimensionInformation> DimensionInformation { get; init; }

#pragma warning restore CS8618
}