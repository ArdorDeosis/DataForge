using Ardalis.GuardClauses;

namespace GraphCreation;

public sealed class StarIndex : IEquatable<StarIndex>
{
  public int Ray { get; }
  public int Distance { get; }

  public bool IsCenter => Distance == 0;

  public StarIndex()
  { }

  public StarIndex(int ray, int distance)
  {
    Ray = Guard.Against.Negative(ray);
    Distance = Guard.Against.NegativeOrZero(distance);
  }

  /// <inheritdoc />
  public bool Equals(StarIndex? other)
  {
    if (ReferenceEquals(null, other))
      return false;
    if (ReferenceEquals(this, other))
      return true;
    return Ray == other.Ray && Distance == other.Distance;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is StarIndex other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() => HashCode.Combine(Ray, Distance);
}