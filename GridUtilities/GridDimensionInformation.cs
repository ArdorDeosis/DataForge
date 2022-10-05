using Utilities;

namespace GridUtilities;

/// <summary>
/// Information about one dimension in a grid containing the grid size in that dimension an its coordinate offset.
/// </summary>
/// TODO: use field keyword
public readonly struct GridDimensionInformation
{
  private readonly int size = 1;

  /// <summary> The length of the grid in this dimension. </summary>
  public /*required*/ int Size
  {
    get => size;
    init
    {
      if (value < 1)
        throw new ArgumentException($"{nameof(size)} is less than 1");
      size = value;
    }
  }

  /// <summary> The offset of the grid coordinates in this dimension. </summary>
  public int Offset { get; init; } = 0;

  /// <summary> Whether the grid wraps around in this dimension. </summary>
  public bool Wrap { get; init; } = false;

  public GridDimensionInformation() =>
    throw ExceptionHelper.StructInvalidWithDefaultValuesException(nameof(GridDimensionInformation));

  // TODO: not sure about this hybrid constructor/initializer approach
  public GridDimensionInformation(int size)
  {
    Size = size;
  }
}