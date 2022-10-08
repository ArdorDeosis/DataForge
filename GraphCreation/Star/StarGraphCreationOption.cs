using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
// TODO: use field keyword
public sealed class StarGraphCreationOption<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOption<StarIndex, TNodeData, TEdgeData>
{
  private readonly int rayCount;

  public /*required*/ int RayCount
  {
    get => rayCount;
    init => rayCount = Guard.Against.Negative(value, nameof(RayCount));
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

#pragma warning disable CS8618 // should go away with the use of the required keyword
  public /*required*/ Func<int, int> CalculateRayLength { get; init; }
#pragma warning restore CS8618
}