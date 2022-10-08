using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
// TODO: use field keyword
public sealed class CompleteGraphCreationOption<TNodeData, TEdgeData>
  : UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // this should be unnecessary once the required keyword is in use
  public /*required*/ IEnumerable<TNodeData> NodeData { get; init; }
#pragma warning restore CS8618

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}