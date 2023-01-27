using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace GridUtilities;

/// <summary>
/// Information about an edge in a cartesian grid. The lower and upper coordinates are equal in all dimensions except
/// the one indicated by <see cref="DimensionOfChange"/>, in which they differ by 1.
/// </summary>
public sealed class GridEdgeInformation : IEquatable<GridEdgeInformation>
{
  /// <summary> The lower coordinate of the edge. </summary>
  public IReadOnlyList<int> LowerCoordinate { get; }

  /// <summary> The upper coordinate of the edge. </summary>
  public IReadOnlyList<int> UpperCoordinate { get; }

  /// <summary>
  /// The dimension in which the coordinates differ and thus the dimension along which the edge runs.
  /// </summary>
  public int DimensionOfChange { get; }

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
  public bool Equals(GridEdgeInformation? other)
  {
    if (ReferenceEquals(null, other))
      return false;
    if (ReferenceEquals(this, other))
      return true;
    return LowerCoordinate.CoordinatesEqual(other.LowerCoordinate) &&
      UpperCoordinate.CoordinatesEqual(other.UpperCoordinate);
  }

  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj))
      return false;
    if (ReferenceEquals(this, obj))
      return true;
    return obj is GridEdgeInformation other && Equals(other);
  }

  /// <inheritdoc />
  public override int GetHashCode() =>
    HashCode.Combine(
      LowerCoordinate.GetCoordinateHashCode(),
      UpperCoordinate.GetCoordinateHashCode()
    );
}