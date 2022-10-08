using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public abstract class IndexedGraphDataCreationOption<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
#pragma warning disable CS8618 // TODO: These warnings should vanish when the required keyword comes
  public /*required*/ Func<TIndex, TNodeData> CreateNodeData { get; init; }
  public /*required*/ Func<IndexedGraphEdgeDefinition<TIndex, TNodeData>, TEdgeData> CreateEdgeData { get; init; }
#pragma warning restore CS8618
}