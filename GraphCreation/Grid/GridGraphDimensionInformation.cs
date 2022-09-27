namespace GraphCreation;

public class GridGraphDimensionInformation
{
  public /*required*/ int Length { get; init; }
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  public static implicit operator GridGraphDimensionInformation(int length) => new() { Length = length };
}