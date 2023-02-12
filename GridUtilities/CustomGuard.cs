using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace DataForge.GridUtilities;

/// <summary>
/// Custom guard clauses for <see cref="GridUtilities" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedParameter.Global")]
internal static class CustomGuardClauses
{
  /// <summary>
  /// Throws an <see cref="ArgumentNullException" /> if <paramref name="dimensionSize" /> is <tt>null</tt>. Throws an
  /// <see cref="ArgumentException" /> if <paramref name="dimensionSize" /> is empty or if any element of
  /// <see cref="dimensionSize" /> is negative or zero.
  /// </summary>
  /// <returns>
  /// <paramref name="dimensionSize" />
  /// </returns>
  /// <exception cref="ArgumentException"></exception>
  /// <exception cref="ArgumentNullException"></exception>
  internal static IReadOnlyList<int> InvalidDimensionsList(this IGuardClause guardClause,
    IReadOnlyList<int> dimensionSize)
  {
    Guard.Against.NullOrEmpty(dimensionSize);
    for (var n = 0; n < dimensionSize.Count; ++n)
      Guard.Against.NegativeOrZero(dimensionSize[n], $"{nameof(dimensionSize)}[{n}]");
    return dimensionSize;
  }

  /// <summary>
  /// Throws an <see cref="ArgumentException" /> if <paramref name="left" /> and <paramref name="right" /> are of different
  /// lengths.
  /// </summary>
  /// <exception cref="ArgumentException"></exception>
  internal static void DifferentLengths<T1, T2>(this IGuardClause guardClause, IReadOnlyList<T1> left,
    IReadOnlyList<T2> right, [CallerArgumentExpression("left")] string? leftName = null,
    [CallerArgumentExpression("right")] string? rightName = null)
  {
    if (left.Count != right.Count)
      throw new ArgumentException($"Expected parameter {leftName} and parameter {rightName} to have the same length.");
  }
}