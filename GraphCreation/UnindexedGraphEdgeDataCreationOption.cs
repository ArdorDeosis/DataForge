using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public abstract class UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // TODO: These warnings should vanish when the required keyword comes
  public /*required*/ Func<TNodeData, TNodeData, TEdgeData> CreateEdgeData { get; init; }
#pragma warning restore CS8618
}