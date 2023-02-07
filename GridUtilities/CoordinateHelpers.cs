using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace DataForge.GridUtilities;

/// <summary>
/// Helper functions for working with (cartesian) coordinates of arbitrary dimension, represented as integer arrays.
/// </summary>
public static class CoordinateHelpers
{
  /// <summary>
  /// Adds two coordinates represented as <tt>int[]</tt>.
  /// </summary>
  public static int[] AddCoordinates(this IReadOnlyList<int> left, IReadOnlyList<int> right)
  {
    Guard.Against.Null(left);
    Guard.Against.Null(right);
    Guard.Against.DifferentLengths(left, right);

    var result = new int[left.Count];
    for (var i = 0; i < result.Length; i++)
      result[i] = left[i] + right[i];
    return result;
  }

  /// <summary>
  /// Checks equality for two coordinates represented as <tt>int[]</tt>.
  /// </summary>
  public static bool CoordinatesEqual(this IReadOnlyList<int> left, IReadOnlyList<int> right) =>
    (left, right) switch
    {
      (null, null) => true,
      (null, { }) or ({ }, null) => false,
      ({ }, { }) when left.Count != right.Count => false,
      _ => !left.Where((t, i) => t != right[i]).Any(),
    };

  /// <summary>
  /// Gets a hash for a coordinate represented as <tt>int[]</tt> in compliance with the <see cref="CoordinatesEqual"/>
  /// function.
  /// </summary>
  /// <remarks>
  /// This function only produces useful hashes for coordinates with up to 8 dimensions. After that, all further
  /// dimensions are ignored in regards to generating the hash. This could result in suboptimal performance for
  /// retrieving objects from a dictionary for coordinates with more than 8 dimensions. 
  /// </remarks>
  [ExcludeFromCodeCoverage]
  public static int GetCoordinateHashCode(this IReadOnlyList<int> coordinate) =>
    coordinate switch
    {
      null or [] => 0,
      [var a] => a,
      [var a, var b] => HashCode.Combine(a, b),
      [var a, var b, var c] => HashCode.Combine(a, b, c),
      [var a, var b, var c, var d] => HashCode.Combine(a, b, c, d),
      [var a, var b, var c, var d, var e] => HashCode.Combine(a, b, c, d, e),
      [var a, var b, var c, var d, var e, var f] => HashCode.Combine(a, b, c, d, e, f),
      [var a, var b, var c, var d, var e, var f, var g] => HashCode.Combine(a, b, c, d, e, f, g),
      [var a, var b, var c, var d, var e, var f, var g, var h, ..] => HashCode.Combine(a, b, c, d, e, f, g, h),
    };

  /// <summary>
  /// A <see cref="EqualityComparer{T}"/> to compare int arrays representing coordinates.
  /// </summary>
  /// <remarks>
  /// The <see cref="GetHashCode"/> functions only produces useful hashes for coordinates with up to 8 dimensions. After
  /// that, all further dimensions are ignored in regards to generating the hash. This could result in suboptimal
  /// performance for retrieving objects from a dictionary for coordinates with more than 8 dimensions. 
  /// </remarks>
  public class EqualityComparer : EqualityComparer<IReadOnlyList<int>>
  {
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(IReadOnlyList<int>? left, IReadOnlyList<int>? right) =>
      left is not null && right is not null && CoordinatesEqual(left, right);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode(IReadOnlyList<int> coordinate) => GetCoordinateHashCode(coordinate);
  }
}