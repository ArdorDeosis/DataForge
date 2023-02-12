using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for graph with a ring structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
/// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public class RingGraphCreationOptions<TNodeData, TEdgeData> : IndexedGraphDataCreationOptions<int, TNodeData, TEdgeData>
{
  private readonly int size;

  /// <summary>
  /// The number of nodes in the ring.
  /// </summary>
  public /*required*/ int Size
  {
    get => size;
    init => size = Guard.Against.NegativeOrZero(value, nameof(Size));
  }

  /// <summary>
  /// The direction
  /// </summary>
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}