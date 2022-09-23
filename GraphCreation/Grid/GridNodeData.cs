namespace GraphCreation;

public readonly struct GridNodeData
{
  public readonly IReadOnlyList<int> Coordinates;

  internal GridNodeData(IReadOnlyList<int> coordinates)
  {
    Coordinates = coordinates;
  }
}