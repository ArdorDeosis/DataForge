using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for graph with a line structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// /// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public class LineGraphCreationOptions<TNodeData, TEdgeData> : IndexedGraphDataCreationOptions<int, TNodeData, TEdgeData>
{
  private readonly int length;

  /// <summary>
  /// The length of the line.
  /// </summary>
  /// <remarks>Equivalent to the number of nodes in the graph.</remarks>
  public required int Length
  {
    get => length;
    init => length = Guard.Against.NegativeOrZero(value, nameof(Length));
  }

  /// <summary>
  /// Direction in which edges are created in the graph.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the lower to the higher numbered node.
  /// </summary>
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}