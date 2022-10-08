using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

/// TODO: use field keyword
[PublicAPI]
public class LineGraphCreationOption<TNodeData, TEdgeData> : IndexedGraphDataCreationOption<int, TNodeData, TEdgeData>
{
  private readonly int length;

  public /*required*/ int Length
  {
    get => length;
    init => length = Guard.Against.NegativeOrZero(value, nameof(Length));
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}