using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public sealed class
  TreeGraphCreationOptions<TNodeData, TEdgeData> : IndexedGraphDataCreationOption<TreeIndex, TNodeData, TEdgeData>
{
  private readonly int? maxDepth;
#pragma warning disable CS8618 // TODO: These warnings should vanish when the required keyword comes

  /// <summary>
  /// Function to calculate the number of child nodes for a given node.
  /// </summary>
  public /*required*/ Func<TreeIndex, TNodeData, int> CalculateChildNodeCount { get; init; }

#pragma warning restore CS8618

  /// <summary>
  /// Maximum depth of the tree structure.
  /// </summary>
  public int? MaxDepth
  {
    get => maxDepth;
    init => maxDepth = value is null ? null : Guard.Against.Negative(value.Value, nameof(MaxDepth));
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}