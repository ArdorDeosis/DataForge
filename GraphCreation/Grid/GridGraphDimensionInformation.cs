using GridUtilities;

namespace GraphCreation;

public readonly struct GridGraphDimensionInformation
{
  public readonly int Size;
  public readonly int Offset;
  public readonly EdgeDirection EdgeDirection;

  public GridGraphDimensionInformation(int size, EdgeDirection edgeDirection) : this(size, 0, edgeDirection)
  { }

  public GridGraphDimensionInformation(int size, int offset = 0, EdgeDirection edgeDirection = EdgeDirection.Forward)
  {
    if (size < 1)
      throw new ArgumentException($"{nameof(size)} is less than 1");
    Size = size;
    Offset = offset;
    EdgeDirection = edgeDirection;
  }

  public static implicit operator GridGraphDimensionInformation(int size) => new(size);
}

public static class GridGraphDimensionInformationExtensions
{
  public static GridDimensionInformation ToGridDimensionInformation(this GridGraphDimensionInformation info) =>
    new(info.Size, info.Offset);
}