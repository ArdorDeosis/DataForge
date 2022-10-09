using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
// TODO: use field keyword
public sealed class BipartiteGraphCreationOption<TNodeData, TEdgeData>
  : UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // this should be unnecessary once the required keyword is in use
  public /*required*/ IEnumerable<TNodeData> NodeDataSetA { get; init; }
  public /*required*/ IEnumerable<TNodeData> NodeDataSetB { get; init; }
#pragma warning restore CS8618

  public Func<TNodeData, TNodeData, EdgeDirection, bool> CreateEdge { get; init; } = (_, _, direction) =>
    direction == EdgeDirection.Forward;
}