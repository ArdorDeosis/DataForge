using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

/// TODO: use field keyword
[PublicAPI]
public class RingGraphCreationOption<TNodeData, TEdgeData> : IndexedGraphDataCreationOption<int, TNodeData, TEdgeData>
{
  private readonly int size;

  public /*required*/ int Size
  {
    get => size;
    init => size = Guard.Against.NegativeOrZero(value, nameof(Size));
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}