using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

/// TODO: use field keyword
[PublicAPI]
public class LineGraphCreationOption<TNodeData, TEdgeData>
{
  private readonly int length;

  public /*required*/ int Length
  {
    get => length;
    init => length = Guard.Against.NegativeOrZero(value, nameof(Length));
  }


#pragma warning disable CS8618 // TODO: I'd hope these warnings vanish with the required keyword
  public /*required*/ Func<int, TNodeData> CreateNodeData { get; init; }
  public /*required*/ Func<EdgeDefinition<int, TNodeData>, TEdgeData> CreateEdgeData { get; init; }
#pragma warning restore CS8618

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}