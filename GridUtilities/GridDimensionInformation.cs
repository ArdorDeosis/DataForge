﻿using Ardalis.GuardClauses;

namespace DataForge.GridUtilities;

/// <summary>
/// Information about one dimension in a grid containing the grid size in that dimension an its coordinate offset.
/// </summary>
public sealed class GridDimensionInformation
{
  // TODO: not sure about this hybrid constructor/initializer approach
  public GridDimensionInformation(int size)
  {
    Size = Guard.Against.NegativeOrZero(size);
  }

  /// <summary> The length of the grid in this dimension. </summary>
  public int Size { get; }

  /// <summary> The offset of the grid coordinates in this dimension. </summary>
  public int Offset { get; init; }

  /// <summary> Whether the grid wraps around in this dimension. </summary>
  public bool Wrap { get; init; }
}