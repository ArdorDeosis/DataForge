using Ardalis.GuardClauses;
using InvalidOperationException = System.InvalidOperationException;

namespace GraphCreation;

public class GridGraphCreationOption<TNodeData, TEdgeData>
{
  private readonly IReadOnlyList<GridGraphDimensionInformation> dimensionInformation;
  private readonly Func<GridNodeData, TNodeData> createNodeData;
  private readonly Func<GridEdgeData<TNodeData, TEdgeData>, TEdgeData> createEdgeData;

  public /*required*/ IReadOnlyList<GridGraphDimensionInformation> DimensionInformation
  {
    get => dimensionInformation;
    init
    {
      Guard.Against.NullOrEmpty(value, nameof(DimensionInformation));
      dimensionInformation = value;
    }
  }

  public /*required*/ Func<GridNodeData, TNodeData> CreateNodeData
  {
    get => createNodeData;
    init
    {
      Guard.Against.Null(value, nameof(CreateNodeData));
      createNodeData = value;
    }
  }

  public /*required*/ Func<GridEdgeData<TNodeData, TEdgeData>, TEdgeData> CreateEdgeData
  {
    get => createEdgeData;
    init
    {
      Guard.Against.Null(value, nameof(CreateEdgeData));
      createEdgeData = value;
    }
  }
}