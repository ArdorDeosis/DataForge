using Ardalis.GuardClauses;

namespace GraphCreation;

public class LineGraphCreationOption<TNodeData, TEdgeData>
{
  private readonly int length;
  private readonly Func<LineNodeData, TNodeData> createNodeData;
  private readonly Func<LineEdgeData<TNodeData, TEdgeData>, TEdgeData> createEdgeData;

  public /*required*/ int Length
  {
    get => length;
    init
    {
      Guard.Against.NegativeOrZero(value, nameof(Length));
      length = value;
    }
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

  public /*required*/ Func<LineNodeData, TNodeData> CreateNodeData
  {
    get => createNodeData;
    init
    {
      Guard.Against.Null(value, nameof(CreateNodeData));
      createNodeData = value;
    }
  }

  public /*required*/ Func<LineEdgeData<TNodeData, TEdgeData>, TEdgeData> CreateEdgeData
  {
    get => createEdgeData;
    init
    {
      Guard.Against.Null(value, nameof(CreateEdgeData));
      createEdgeData = value;
    }
  }
}