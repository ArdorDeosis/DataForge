using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
// TODO: use field keyword once it is available
public class GridGraphDimensionInformation
{
  private readonly int length = 1;

  public /*required*/ int Length
  {
    get => length;
    init => length = Guard.Against.NegativeOrZero(value, nameof(Length));
  }

  [ExcludeFromCodeCoverage]
  public bool Wrap { get; init; }

  [ExcludeFromCodeCoverage]
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  [ExcludeFromCodeCoverage]
  public static implicit operator GridGraphDimensionInformation(int length) => new() { Length = length };
}