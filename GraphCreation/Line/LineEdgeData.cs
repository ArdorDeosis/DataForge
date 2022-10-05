using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct LineEdgeData<TNodeData>
{
  public /*required*/ int OriginPosition { get; init; }
  public /*required*/ int DestinationPosition { get; init; }
  public /*required*/ TNodeData OriginNodeData { get; init; }
  public /*required*/ TNodeData DestinationNodeData { get; init; }
}