namespace GraphCreation;

public readonly struct EdgeDefinition<TIndex, TNodeData>
{
  public /*required*/ TIndex OriginAddress { get; init; }
  public /*required*/ TIndex DestinationAddress { get; init; }
  public /*required*/ TNodeData OriginNodeData { get; init; }
  public /*required*/ TNodeData DestinationNodeData { get; init; }
}