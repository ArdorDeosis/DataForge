namespace GraphCreation;

public class GridGraphCreationOption<TNodeData, TEdgeData>
{
  public required IReadOnlyList<GridGraphDimensionInformation> DimensionInformation { get; init; }
  public required Func<GridNodeData, TNodeData> CreateNodeData { get; init; }
  public required Func<GridEdgeData<TNodeData, TEdgeData>, TEdgeData> CreateEdgeData { get; init; }

  internal void Validate()
  {
    if (DimensionInformation.Count < 1)
      throw new InvalidOperationException("There are no dimensions defined!");
    if (CreateNodeData is null)
      throw new InvalidOperationException("No method to generate node data is defined!");
    if (CreateEdgeData is null)
      throw new InvalidOperationException("No method to generate edge data is defined!");
  }
}