using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for graph with a disk structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
/// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public sealed class DiskGraphCreationOptions<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOptions<DiskIndex, TNodeData, TEdgeData>
{
  private readonly int meridianCount = 1;
  private readonly int ringCount = 1;

  /// <summary>
  /// The number of meridians (rays from the center) of the graph.
  /// </summary>
  public /*required*/ int MeridianCount
  {
    get => meridianCount;
    init => meridianCount = Guard.Against.NegativeOrZero(value, nameof(MeridianCount));
  }

  /// <summary>
  /// The number of rings around the center.
  /// </summary>
  public /*required*/ int RingCount
  {
    get => ringCount;
    init => ringCount = Guard.Against.NegativeOrZero(value, nameof(RingCount));
  }

  /// <summary>
  /// Direction in which edges should be created along the meridians.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the center outwards.
  /// </summary>
  public EdgeDirection MeridianEdgeDirection { get; init; } = EdgeDirection.Forward;

  /// <summary>
  /// Direction in which edges should be created along the rings.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the lower numbered meridian to the higher.
  /// </summary>
  public EdgeDirection RingEdgeDirection { get; init; } = EdgeDirection.Forward;

  /// <summary>
  /// Whether the graph should contain a center node.
  /// </summary>
  public bool MakeCenterNode { get; init; } = true;
}