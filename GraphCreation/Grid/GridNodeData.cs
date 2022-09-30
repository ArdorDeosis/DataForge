using Utilities;

namespace GraphCreation;

public readonly struct GridNodeData
{
  public readonly IReadOnlyList<int> Coordinates;

  public GridNodeData() => ThrowHelper.ThrowStructNotPubliclyConstructableException(nameof(GridNodeData));

  internal GridNodeData(IReadOnlyList<int> coordinates)
  {
    Coordinates = coordinates;
  }
}