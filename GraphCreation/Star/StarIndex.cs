using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// An index for a node in a star graph.
/// </summary>
[PublicAPI]
public sealed class StarIndex : IEquatable<StarIndex>
{
  /// <summary>
  /// The number of the ray the node is on.
  /// </summary>
  public int Ray { get; }

  /// <summary>
  /// The from the center to the node on its ray.
  /// </summary>
  public int Distance { get; }

  /// <summary>
  /// Whether the node is the center node of the star graph.
  /// </summary>
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