using GridUtilities;

namespace GraphCreation;

public class GridGraphDimensionInformation
{
  public required int Size { get; init; }
  public int Offset { get; init; } = 0;
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  public static implicit operator GridGraphDimensionInformation(int size) => new() { Size = size };
}

public static class GridGraphDimensionInformationExtensions
{
  public static GridDimensionInformation ToGridDimensionInformation(this GridGraphDimensionInformation info) =>
    new(info.Size, info.Offset);
}