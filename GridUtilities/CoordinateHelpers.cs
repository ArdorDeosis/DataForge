using System.Diagnostics.CodeAnalysis;

namespace GridUtilities;

/// <summary>
/// Helper functions for working with (cartesian) coordinates of arbitrary dimension, represented as integer arrays.
/// </summary>
public static class CoordinateHelpers
{
  /// <summary>
  /// Adds two coordinates represented as <tt>int[]</tt>.
  /// </summary>
  public static int[] AddCoordinates(IReadOnlyList<int> left, IReadOnlyList<int> right)
  {
    var result = new int[left.Count];
    for (var i = 0; i < result.Length; i++)
      result[i] = left[i] + right[i];
    return result;
  }
  
  /// <summary>
  /// Checks equality for two coordinates represented as <tt>int[]</tt>.
  /// </summary>
  public static bool CoordinatesEqual(IReadOnlyList<int> left, IReadOnlyList<int> right) => 
    left.Count == right.Count && !left.Where((t, i) => t != right[i]).Any();

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
  public static int GetCoordinateHashCode(IReadOnlyList<int> obj) =>
    obj switch
    {
      [] => 0,
      [var a] => a,
      [var a, var b] => HashCode.Combine(a, b),
      [var a, var b, var c] => HashCode.Combine(a, b, c),
      [var a, var b, var c, var d] => HashCode.Combine(a, b, c, d),
      [var a, var b, var c, var d, var e] => HashCode.Combine(a, b, c, d, e),
      [var a, var b, var c, var d, var e, var f] => HashCode.Combine(a, b, c, d, e, f),
      [var a, var b, var c, var d, var e, var f, var g] => HashCode.Combine(a, b, c, d, e, f, g),
      [var a, var b, var c, var d, var e, var f, var g, var h, ..] => HashCode.Combine(a, b, c, d, e, f, g, h),
    };
}