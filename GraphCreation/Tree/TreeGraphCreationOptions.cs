using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for a tree graph.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// /// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public sealed class
  TreeGraphCreationOptions<TNodeData, TEdgeData> : IndexedGraphDataCreationOptions<TreeIndex, TNodeData, TEdgeData>
{
  private readonly int? maxDepth;

  /// <summary>
  /// Function to calculate the number of child nodes for a given node.
  /// </summary>
  public required Func<TreeIndex, TNodeData, int> CalculateChildNodeCount { get; init; }


  /// <summary>
  /// Optional maximum depth of the tree structure.
  /// </summary>
  /// <remarks>
  /// If this is not set, the tree generation can potentially be endless, depending on
  /// <see cref="CalculateChildNodeCount" />.
  /// </remarks>
  public int? MaxDepth
  {
    get => maxDepth;
    init => maxDepth = value is null ? null : Guard.Against.Negative(value.Value, nameof(MaxDepth));
  }

  /// <summary>
  /// Direction in which edges are created.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the node closer to the root to the node
  /// further down the tree.
  /// </summary>
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}