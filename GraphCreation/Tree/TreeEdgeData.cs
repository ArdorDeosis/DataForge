using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct TreeEdgeData<TNodeData>
{
  public /*required*/ IReadOnlyList<int> OriginAddress { get; init; }
  public /*required*/ IReadOnlyList<int> DestinationAddress { get; init; }
  public /*required*/ TNodeData OriginNodeData { get; init; }
  public /*required*/ TNodeData DestinationNodeData { get; init; }
}