using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public class TreeGraphCreationOptions<TNodeData, TEdgeData>
{
  private readonly int? maxDepth;
#pragma warning disable CS8618 // TODO: I'd hope these warnings vanish with the required keyword

  /// <summary>
  /// Function to calculate the number of child nodes for a given node.
  /// </summary>
  public /*required*/ Func<TreeNodeData, TNodeData, int> CalculateChildNodeCount { get; init; }

  /// <summary>
  /// Function to create the node data.
  /// </summary>
  public /*required*/ Func<TreeNodeData, TNodeData> CreateNodeData { get; init; }

  /// <summary>
  /// Function to create the edge data.
  /// </summary>
  public /*required*/ Func<TreeEdgeData<TNodeData>, TEdgeData> CreateEdgeData { get; init; }

#pragma warning restore CS8618

  /// <summary>
  /// Maximum depth of the tree structure.
  /// </summary>
  public int? MaxDepth
  {
    get => maxDepth;
    init => maxDepth = value is null ? null : Guard.Against.NegativeOrZero(value.Value, nameof(MaxDepth));
  }

  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}