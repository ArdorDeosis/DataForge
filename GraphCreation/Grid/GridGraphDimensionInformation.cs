using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Information about one dimension in a grid graph.
/// </summary>
/// /// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public class GridGraphDimensionInformation
{
  private readonly int length = 1;

  /// <summary>
  /// Length of this dimension.
  /// </summary>
  public required int Length
  {
    get => length;
    init => length = Guard.Against.NegativeOrZero(value, nameof(Length));
  }

  /// <summary>
  /// Whether edges should be created between the last and first nodes in this dimension.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public bool Wrap { get; init; }

  /// <summary>
  /// Direction in which edges should be created in this dimension.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the lower to the higher numbered node in
  /// this dimension.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  /// <summary>
  /// Implicitly converts an integer to a <see cref="GridGraphDimensionInformation" /> with the length of that integer
  /// and default values for all other parameters.
  /// </summary>
  /// <param name="length">The length of the produced dimension information.</param>
  [ExcludeFromCodeCoverage]
  public static implicit operator GridGraphDimensionInformation(int length) =>
    new() { Length = length };
}