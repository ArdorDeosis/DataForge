using Ardalis.GuardClauses;

namespace GraphCreation.Disk;

public sealed class DiskIndex : IEquatable<DiskIndex>
{
  public int Meridian { get; }
  public int Distance { get; }

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