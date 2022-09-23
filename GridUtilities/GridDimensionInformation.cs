namespace GridUtilities;

/// <summary>
/// Information about one dimension in a grid containing the grid size in that dimension an its coordinate offset.
/// </summary>
public readonly struct GridDimensionInformation
{
  /// <summary> The length of the grid in this dimension. </summary>
  public readonly int Size;
  /// <summary> The offset of the grid coordinates in this dimension. </summary>
  public readonly int Offset;
  
  public GridDimensionInformation(int size, int offset = 0)
  {
    if (size < 1)
      throw new ArgumentException($"{nameof(size)} is less than 1");
    Size = size;
    Offset = offset;
  }
}