using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public class GridGraphDimensionInformation
{
  public /*required*/ int Length { get; init; }
  public bool Wrap { get; init; } = false;
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  public static implicit operator GridGraphDimensionInformation(int length) => new() { Length = length };
}