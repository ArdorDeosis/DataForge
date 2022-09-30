using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using Utilities;

namespace GridUtilities;

/// <summary>
/// Information about an edge in a cartesian grid. The lower and upper coordinates are equal in all dimensions except
/// the one indicated by <see cref="DimensionOfChange"/>, in which they differ by 1.
/// </summary>
public readonly struct GridEdgeInformation : IEquatable<GridEdgeInformation>
{
  /// <summary> The lower coordinate of the edge. </summary>
  public readonly IReadOnlyList<int> LowerCoordinate;

  /// <summary> The upper coordinate of the edge. </summary>
  public readonly IReadOnlyList<int> UpperCoordinate;

  /// <summary>
  /// The dimension in which the coordinates differ and thus the dimension along which the edge runs.
  /// </summary>
  public readonly int DimensionOfChange;

  public GridEdgeInformation() => ThrowHelper.ThrowStructNotPubliclyConstructableException(nameof(GridEdgeInformation));

  internal GridEdgeInformation(IReadOnlyList<int> lowerCoordinate, int dimensionOfChange)
  {
    Guard.Against.NullOrEmpty(lowerCoordinate);
    Guard.Against.OutOfRange(dimensionOfChange, nameof(dimensionOfChange), 0, lowerCoordinate.Count);

    LowerCoordinate = lowerCoordinate;
    var upperCoordinate = lowerCoordinate.ToArray();
    ++upperCoordinate[dimensionOfChange];
    UpperCoordinate = upperCoordinate;
    DimensionOfChange = dimensionOfChange;
  }

  /// <summary>
  /// This constructor can be used for wrapped edges. For normal edges, use the constructor that calculates the upper
  /// coordinate. This just can't be done for wrapping dimensions, as the information about the grid size in a
  /// particular dimension is not available here.
  /// </summary>
  internal GridEdgeInformation(IReadOnlyList<int> lowerCoordinate, IReadOnlyList<int> upperCoordinate,
    int dimensionOfChange)
  {
    Guard.Against.NullOrEmpty(lowerCoordinate);
    Guard.Against.OutOfRange(dimensionOfChange, nameof(dimensionOfChange), 0, lowerCoordinate.Count);

    LowerCoordinate = lowerCoordinate;
    UpperCoordinate = upperCoordinate;
    DimensionOfChange = dimensionOfChange;
  }

  [ExcludeFromCodeCoverage]
  public static bool operator ==(GridEdgeInformation left, GridEdgeInformation right) => left.Equals(right);

  [ExcludeFromCodeCoverage]
  public static bool operator !=(GridEdgeInformation left, GridEdgeInformation right) => !(left == right);

  [ExcludeFromCodeCoverage]
  public override string ToString() =>
    $"([{string.Join(", ", LowerCoordinate)}] => [{string.Join(", ", UpperCoordinate)}]; {DimensionOfChange})";

  /// <inheritdoc />
  public bool Equals(GridEdgeInformation other) =>
    LowerCoordinate.CoordinatesEqual(other.LowerCoordinate) &&
    DimensionOfChange == other.DimensionOfChange;

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is GridEdgeInformation other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() =>
    HashCode.Combine(
      LowerCoordinate.GetCoordinateHashCode(),
      DimensionOfChange
    );
}