﻿using Ardalis.GuardClauses;

namespace GridUtilities;

/// <summary>
/// Helper functions to produce or iterate grid information. 
/// </summary>
public static class Grid
{
  /// <summary>
  /// Information about all nodes in a cartesian grid.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="size"/> parameter. Every
  /// value defines the size of the grid in the corresponding dimension. Dimension n's coordinate range is thus
  /// defined as <tt>[0, size[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="size">
  /// The size of the grid in the corresponding dimensions. This also defines the number of dimensions of the grid.
  /// </param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of all node coordinates in the grid, each represented as <tt>int[]</tt>.
  /// </returns>
  /// <exception cref="ArgumentException">
  /// If <paramref name="size"/> is empty or if any value of size is less than 1.
  /// </exception>
  public static IEnumerable<int[]> Coordinates(IReadOnlyList<int> size)
  {
    Guard.Against.InvalidDimensionsList(size);
    return CoordinatesInternal(size.Select(dimension => new GridDimensionInformation(dimension)).ToArray());
  }

  /// <inheritdoc cref="Coordinates(System.Collections.Generic.IReadOnlyList{int})"/>
  public static IEnumerable<int[]> Coordinates(params int[] size)
  {
    Guard.Against.InvalidDimensionsList(size);
    return CoordinatesInternal(size.Select(dimension => new GridDimensionInformation(dimension)).ToArray());
  }

  /// <summary>
  /// Information about all nodes in a cartesian grid.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="size"/> parameter. Every
  /// value defines the size of the grid in the corresponding dimension. The <paramref name="offset"/> parameter
  /// defines an offset to the grid in the corresponding dimensions. Dimension n's coordinate range is thus defined as
  /// <tt>[offset[n], size[n] + offset[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="size">
  /// The size of the grid in the corresponding dimensions. This also defines the number of dimensions of the grid.
  /// </param>
  /// <param name="offset">The offset of the grid coordinates in the corresponding dimensions.</param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of all node coordinates in the grid, each represented as <tt>int[]</tt>.
  /// </returns>
  /// <remarks>
  /// The <paramref name="size"/> and <paramref name="offset"/> parameters need to have the same length.
  /// </remarks>
  /// <exception cref="ArgumentException">
  /// If <paramref name="size"/> is empty, if any value of size is less than 1, or if <paramref name="offset"/> has a
  /// different length than <paramref name="size"/>.
  /// </exception>
  public static IEnumerable<int[]> Coordinates(IReadOnlyList<int> size, IReadOnlyList<int> offset)
  {
    Guard.Against.InvalidDimensionsList(size);
    Guard.Against.DifferentLengths(size, offset);
    var gridDimensionData = new GridDimensionInformation[size.Count];
    for (var n = 0; n < size.Count; n++)
      gridDimensionData[n] = new GridDimensionInformation(size[n], offset[n]);
    return CoordinatesInternal(gridDimensionData);
  }

  /// <inheritdoc cref="CoordinatesInternal"/>
  /// <exception cref="ArgumentException">If <paramref name="dimensions"/> is empty.</exception>
  public static IEnumerable<int[]> Coordinates(IReadOnlyList<GridDimensionInformation> dimensions)
  {
    Guard.Against.NullOrEmpty(dimensions);
    return CoordinatesInternal(dimensions);
  }

  /// <inheritdoc cref="CoordinatesInternal"/>
  /// <exception cref="ArgumentException">If <paramref name="dimensions"/> is empty.</exception>
  public static IEnumerable<int[]> Coordinates(params GridDimensionInformation[] dimensions)
  {
    Guard.Against.NullOrEmpty(dimensions);
    return CoordinatesInternal(dimensions);
  }

  /// <summary>
  /// Information about all edges in a cartesian grid. Edges are drawn between all nodes in the grid that are offset by
  /// 1 from each other in one dimension.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="size"/> parameter. Every
  /// value defines the size of the grid in the corresponding dimension. Dimension n's coordinate range is thus
  /// defined as <tt>[0, size[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="size">
  /// The size of the grid in the corresponding dimensions. This also defines the number of dimensions of the grid.
  /// </param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of <see cref="GridEdgeInformation"/> containing the coordinates the edge is
  /// connecting and the dimension which separates these coordinates.
  /// </returns>
  /// <exception cref="ArgumentException">
  /// If <paramref name="size"/> is empty or if any value of size is less than 1.
  /// </exception>
  public static IEnumerable<GridEdgeInformation> EdgeInformation(IReadOnlyList<int> size)
  {
    Guard.Against.InvalidDimensionsList(size);
    return EdgeInformationInternal(
      size.Select(dimension => new GridDimensionInformation(dimension)).ToArray());
  }

  /// <inheritdoc cref="EdgeInformation(System.Collections.Generic.IReadOnlyList{int})"/>
  public static IEnumerable<GridEdgeInformation> EdgeInformation(params int[] size)
  {
    Guard.Against.InvalidDimensionsList(size);
    return EdgeInformationInternal(
      size.Select(dimension => new GridDimensionInformation(dimension)).ToArray());
  }

  /// <summary>
  /// Information about all edges in a cartesian grid. Edges are drawn between all nodes in the grid that are offset by
  /// 1 from each other in one dimension.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="size"/> parameter. Every
  /// value defines the size of the grid in the corresponding dimension. The <paramref name="offset"/> parameter
  /// defines an offset to the grid in the corresponding dimensions. Dimension n's coordinate range is thus defined as
  /// <tt>[offset[n], size[n] + offset[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="size">
  /// The size of the grid in the corresponding dimensions. This also defines the number of dimensions of the grid.
  /// </param>
  /// <param name="offset">The offset of the grid coordinates in the corresponding dimensions.</param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of <see cref="GridEdgeInformation"/> containing the coordinates the edge is
  /// connecting and the dimension which separates these coordinates.
  /// </returns>
  /// <remarks>
  /// The <paramref name="size"/> and <paramref name="offset"/> parameters need to have the same length.
  /// </remarks>
  /// <exception cref="ArgumentException">
  /// If <paramref name="size"/> is empty, if any value of size is less than 1, or if <paramref name="offset"/> has a
  /// different length than <paramref name="size"/>.
  /// </exception>
  public static IEnumerable<GridEdgeInformation> EdgeInformation(IReadOnlyList<int> size, IReadOnlyList<int> offset)
  {
    Guard.Against.InvalidDimensionsList(size);
    Guard.Against.DifferentLengths(size, offset);
    var gridDimensionData = new GridDimensionInformation[size.Count];
    for (var n = 0; n < size.Count; n++)
      gridDimensionData[n] = new GridDimensionInformation(size[n], offset[n]);
    return EdgeInformationInternal(gridDimensionData);
  }

  /// <inheritdoc cref="EdgeInformationInternal"/>
  /// <exception cref="ArgumentException">If <paramref name="dimensions"/> is empty.</exception>
  public static IEnumerable<GridEdgeInformation> EdgeInformation(IReadOnlyList<GridDimensionInformation> dimensions)
  {
    Guard.Against.NullOrEmpty(dimensions);
    return EdgeInformationInternal(dimensions);
  }

  /// <inheritdoc cref="EdgeInformationInternal"/>
  /// <exception cref="ArgumentException">If <paramref name="dimensions"/> is empty.</exception>
  public static IEnumerable<GridEdgeInformation> EdgeInformation(params GridDimensionInformation[] dimensions)
  {
    Guard.Against.NullOrEmpty(dimensions);
    return EdgeInformationInternal(dimensions);
  }

  /// <summary>
  /// Information about all nodes in a cartesian grid.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="dimensions"/> parameter. Every
  /// value defines the <see cref="GridDimensionInformation.Size">size</see> of the grid and the
  /// <see cref="GridDimensionInformation.Offset">offset</see> of the grid coordinates in the corresponding dimension.
  /// Dimension n's coordinate range is thus defined as <tt>[offset[n], size[n] + offset[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="dimensions">The dimension information defining the grid.</param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of all node coordinates in the grid, each represented as <tt>int[]</tt>.
  /// </returns>
  private static IEnumerable<int[]> CoordinatesInternal(IReadOnlyList<GridDimensionInformation> dimensions)
  {
    var offset = dimensions.Select(info => info.Offset).ToArray();
    var currentCoordinate = new int[dimensions.Count];
    Array.Fill(currentCoordinate, 0);

    while (true)
    {
      yield return CoordinateHelpers.AddCoordinates(currentCoordinate, offset);

      if (IsCurrentCoordinateLastCoordinate())
        yield break;

      AdvanceCurrentCoordinate();
    }

    // checks if the iteration reached the last coordinate to iterate
    bool IsCurrentCoordinateLastCoordinate()
    {
      for (var n = 0; n < dimensions.Count; ++n)
      {
        if (currentCoordinate[n] != dimensions[n].Size - 1)
          return false;
      }

      return true;
    }

    // advances the current coordinate
    void AdvanceCurrentCoordinate()
    {
      var currentDigit = 0;
      while (currentDigit < dimensions.Count && ++currentCoordinate[currentDigit] >= dimensions[currentDigit].Size)
        currentCoordinate[currentDigit++] = 0;
    }
  }

  /// <summary>
  /// Information about all edges in a cartesian grid. Edges are drawn between all nodes in the grid that are offset by
  /// 1 from each other in one dimension.
  /// <br/><br/>
  /// <b>Grid Definition</b>
  /// <p>The grid's number of dimensions is defined by the length of the <paramref name="dimensions"/> parameter. Every
  /// value defines the <see cref="GridDimensionInformation.Size">size</see> of the grid and the
  /// <see cref="GridDimensionInformation.Offset">offset</see> of the grid coordinates in the corresponding dimension.
  /// Dimension n's coordinate range is thus defined as <tt>[offset[n], size[n] + offset[n] - 1]</tt>.</p>
  /// </summary>
  /// <param name="dimensions">The grid dimension information defining the grid.</param>
  /// <returns>
  /// An <see cref="IEnumerable{T}"/> of <see cref="GridEdgeInformation"/> containing the coordinates the edge is
  /// connecting and the dimension which separates these coordinates.
  /// </returns>
  private static IEnumerable<GridEdgeInformation> EdgeInformationInternal(
    IReadOnlyList<GridDimensionInformation> dimensions)
  {
    foreach (var coordinates in CoordinatesInternal(dimensions))
    {
      for (var dimension = 0; dimension < dimensions.Count; dimension++)
      {
        if (coordinates[dimension] >=
            dimensions[dimension].Size + dimensions[dimension].Offset - 1)
          continue;

        yield return new GridEdgeInformation(coordinates, dimension);
      }
    }
  }
}