using System.Diagnostics.CodeAnalysis;

namespace GraphCreation;

[ExcludeFromCodeCoverage]
public readonly struct IndexedGraphEdgeDefinition<TIndex, TNodeData> where TIndex : notnull
{
  public /*required*/ TIndex OriginIndex { get; init; }
  public /*required*/ TIndex DestinationIndex { get; init; }
  public /*required*/ TNodeData OriginNodeData { get; init; }
  public /*required*/ TNodeData DestinationNodeData { get; init; }
}