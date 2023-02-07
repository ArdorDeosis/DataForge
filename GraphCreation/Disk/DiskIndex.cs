using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Index of a node in a disk graph.
/// </summary>
[PublicAPI]
public sealed class DiskIndex : IEquatable<DiskIndex>
{
  /// <summary>
  /// The meridian the node is on.
  /// </summary>
  public int Meridian { get; }
  
  /// <summary>
  /// The distance the node is from the center. This corresponds with the ring the node is on. But the center node (and
  /// only the center node) has a distance of one, the rings are outward numbered from 1 to incl.
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.RingCount"/>.
  /// </summary>
  public int Distance { get; }

  /// <summary>
  /// Whether the node is the center node of the graph.
  /// </summary>
  public bool IsCenter => Distance == 0;

  public DiskIndex()
  { }

  public DiskIndex(int meridian, int distance)
  {
    Meridian = Guard.Against.Negative(meridian);
    Distance = Guard.Against.NegativeOrZero(distance);
  }

  /// <inheritdoc />
  public bool Equals(DiskIndex? other)
  {
    if (ReferenceEquals(null, other))
      return false;
    if (ReferenceEquals(this, other))
      return true;
    return Meridian == other.Meridian && Distance == other.Distance;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is DiskIndex other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() => HashCode.Combine(Meridian, Distance);
}