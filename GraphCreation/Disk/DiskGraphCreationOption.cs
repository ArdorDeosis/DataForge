using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
// TODO: use field keyword
public sealed class DiskGraphCreationOption<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOption<DiskIndex, TNodeData, TEdgeData>
{
  // TODO: these default values should go once the required keyword is in use
  private readonly int meridianCount = 1;
  private readonly int ringCount = 1;

  public /*required*/ int MeridianCount
  {
    get => meridianCount;
    init => meridianCount = Guard.Against.NegativeOrZero(value, nameof(MeridianCount));
  }

  public /*required*/ int RingCount
  {
    get => ringCount;
    init => ringCount = Guard.Against.NegativeOrZero(value, nameof(RingCount));
  }

  public EdgeDirection MeridianEdgeDirection { get; init; } = EdgeDirection.Forward;
  public EdgeDirection RingEdgeDirection { get; init; } = EdgeDirection.Forward;

  public bool MakeCenterNode { get; init; } = true;
}