using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct GridEdgeData<TNodeData>
{
  public /*required*/ IReadOnlyList<int> OriginCoordinate { get; init; }
  public /*required*/ IReadOnlyList<int> DestinationCoordinate { get; init; }
  public /*required*/ TNodeData OriginNodeData { get; init; }
  public /*required*/ TNodeData DestinationNodeData { get; init; }
}